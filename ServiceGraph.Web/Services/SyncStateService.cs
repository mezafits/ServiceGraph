using ServiceGraph.Common;
// Singleton stateful service
public class SyncStateService : ISyncStateService
{
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private readonly ILogger<SyncStateService> _logger;
    private bool _initialized;

    public SyncStateService(ILogger<SyncStateService> logger)
    {
        _logger = logger;
    }

    public Dictionary<string, Project> Projects { get; } = new();
    public Guid SelectedProjectId { get; set; }
    public Project SelectedProject
    {
        get
        {
            try
            {
                if (SelectedProjectId == Guid.Empty) return Projects.FirstOrDefault().Value;
                if (Projects.TryGetValue(SelectedProjectId.ToString(), out var project))
                {
                    return project;
                }
                throw new KeyNotFoundException($"Project with ID {SelectedProjectId} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving selected project with ID {ProjectId}", SelectedProjectId);
                throw;
            }
        }
    }

    public async Task InitializeAsync(string userName, IServiceClient serviceClient)
    {
        if (_initialized) return;

        try
        {
            await _initLock.WaitAsync();
            if (_initialized) return;
            _logger.LogInformation("Initializing SyncStateService for user {UserName}", userName);

            await RefreshProjectsAsync(userName, serviceClient);
            _initialized = true;
            _logger.LogInformation("Initialized SyncStateService for user {UserName}", userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize SyncStateService for user {UserName}", userName);
            throw;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task RefreshProjectsAsync(string userName, IServiceClient serviceClient)
    {
        try
        {
            _logger.LogInformation("Refreshing projects for user {UserName}", userName);
            var projects = await serviceClient.GetProjectsAsync(userName);
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects[project.Id.ToString()] = project;
            }
            _logger.LogInformation("Refreshed {Count} projects for user {userName}", projects.Count, userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh projects for user {UserName}", userName);
            throw new InvalidOperationException("Failed to refresh projects", ex);
        }
    }
}
