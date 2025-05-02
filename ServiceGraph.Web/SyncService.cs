using Microsoft.AspNetCore.Components.Authorization;
using ServiceGraph.Common;
public interface ISyncStateService
{
    Dictionary<string, Project> Projects { get; }
    Guid SelectedProjectId { get; set; }
    Project SelectedProject { get; }
    Task InitializeAsync(string userName, ServiceClient serviceClient);
    Task RefreshProjectsAsync(string userName, ServiceClient serviceClient);
}

public interface ISyncService
{
    Task<Project> GetSelectedProjectAsync();
    Task SetSelectedProjectAsync(string projectId);
    Task UpsertProjectAsync(Project project);
    Task UpsertServiceNodeAsync(ServiceNode node);
    Task UpsertEdgeAsync(Edge edge);
    Task RemoveServiceNodeAsync(ServiceNode node);
    Task RemoveEdgeAsync(Edge edge);
    Task<ServiceNode> AddServiceNodeToGroupAsync(Guid projectId, Guid parentId, Guid serviceNodeId);
    Task<ServiceNode> GetServiceNodeAsync(ServiceNode node);
    Task<ServiceNode> GetServiceNodeByIdAsync(Guid projectId, Guid serviceNodeId);
    Task<Edge> GetEdgeAsync(Edge edge);
    Task<List<Project>> GetProjectsAsync();
    Task<bool> SyncAsync(Project project);
    Task<List<SvgFileInfo>> GetIcons();
}

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

    public async Task InitializeAsync(string userName, ServiceClient serviceClient)
    {
        if (_initialized) return;

        try
        {
            await _initLock.WaitAsync();
            if (_initialized) return;

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

    public async Task RefreshProjectsAsync(string userName, ServiceClient serviceClient)
    {
        try
        {
            var projects = await serviceClient.GetProjectsAsync(userName);
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects[project.Id.ToString()] = project;
            }
            _logger.LogInformation("Refreshed {Count} projects for user {UserName}", projects.Count, userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh projects for user {UserName}", userName);
            throw new InvalidOperationException("Failed to refresh projects", ex);
        }
    }
}

// scoped service
public class SyncService : ISyncService
{
    private readonly ISyncStateService _state;
    private readonly IHttpClientFactory _clientFactory;
    private readonly AuthenticationStateProvider _authProvider;
    private readonly ILogger<SyncService> _logger;

    private ServiceClient _client;
    private string _userName;

    public SyncService(
        ISyncStateService state,
        IHttpClientFactory clientFactory,
        AuthenticationStateProvider authProvider,
        ILogger<SyncService> logger)
    {
        _state = state;
        _clientFactory = clientFactory;
        _authProvider = authProvider;
        _logger = logger;
    }

    private async Task EnsureInitializedAsync()
    {
        try
        {
            if (_client != null) return;

            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
                throw new InvalidOperationException("User is not authenticated");

            _userName = user.Identity.Name ?? user.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value
                ?? throw new InvalidOperationException("Unable to determine user identity");

            _client = new ServiceClient(_clientFactory.CreateClient("ServiceClient"));
            await _state.InitializeAsync(_userName, _client);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during service initialization");
            throw;
        }
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            await _state.RefreshProjectsAsync(_userName, _client);
            return _state.Projects.Values.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get projects");
            throw;
        }
    }

    public async Task<Project> GetSelectedProjectAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.SelectedProjectId == null && _state.Projects.Count > 0)
            {
                _state.SelectedProjectId = _state.Projects.First().Value.Id;
            }

            return _state.SelectedProject ?? throw new InvalidOperationException("No selected project");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get selected project");
            throw;
        }
    }

    public async Task SetSelectedProjectAsync(string projectId)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(projectId, out var project))
            {
                _state.SelectedProjectId = project.Id;
                _logger.LogInformation("Selected project set to {ProjectId}", project.Id);
            }
            else
            {
                throw new KeyNotFoundException($"Project with ID {projectId} not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set selected project ID {ProjectId}", projectId);
            throw;
        }
    }

    public async Task UpsertProjectAsync(Project project)
    {
        try
        {
            await EnsureInitializedAsync();
            var result = await _client.UpsertProject(project);
            if (result.HasError)
            {
                _logger.LogError("Error upserting project {ProjectId}: {Error}", project.Id, result.Errors.First().Message);
                throw new InvalidOperationException(result.Errors.First().Message);
            }
            _logger.LogInformation("Upserted project {ProjectId}", project.Id);
            _state.Projects[project.Id.ToString()] = project;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upsert project {ProjectId}", project.Id);
            throw;
        }
    }

    public async Task UpsertServiceNodeAsync(ServiceNode node)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(node.ProjectId.ToString(), out var project))
            {
                project.UpsertServiceNode(node);
                await _client.UpsertProject(project);
                _logger.LogInformation("Upserted service node {NodeId} in project {ProjectId}", node.Id, node.ProjectId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upsert service node {NodeId}", node.Id);
            throw;
        }
    }

    public async Task UpsertEdgeAsync(Edge edge)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(edge.ProjectId.ToString(), out var project))
            {
                project.UpsertEdge(edge);
                await _client.UpsertProject(project);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upsert edge {EdgeId}", edge.Id);
            throw;
        }
    }

    public async Task RemoveServiceNodeAsync(ServiceNode node)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(node.ProjectId.ToString(), out var project))
            {
                project.RemoveServiceNode(node);
                await _client.UpsertProject(project);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove service node {NodeId}", node.Id);
            throw;
        }
    }

    public async Task RemoveEdgeAsync(Edge edge)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(edge.ProjectId.ToString(), out var project))
            {
                project.RemoveEdge(edge);
                await _client.UpsertProject(project);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove edge {EdgeId}", edge.Id);
            throw;
        }
    }

    public async Task<ServiceNode> AddServiceNodeToGroupAsync(Guid projectId, Guid parentId, Guid serviceNodeId)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(projectId.ToString(), out var project))
            {
                var node = project.nodes?.FirstOrDefault(n => n.Id == serviceNodeId);
                if (node != null)
                {
                    node.ParentId = parentId;
                    await _client.UpsertProject(project);
                    return node;
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add service node {NodeId} to group {ParentId}", serviceNodeId, parentId);
            throw;
        }
    }

    public async Task<ServiceNode> GetServiceNodeAsync(ServiceNode node)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(node.ProjectId.ToString(), out var project))
            {
                return project.nodes?.FirstOrDefault(n => n.Id == node.Id);
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get service node {NodeId}", node.Id);
            throw;
        }
    }

    public async Task<ServiceNode> GetServiceNodeByIdAsync(Guid projectId, Guid serviceNodeId)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(projectId.ToString(), out var project))
            {
                return project.nodes?.FirstOrDefault(n => n.Id == serviceNodeId);
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get service node by ID {ServiceNodeId}", serviceNodeId);
            throw;
        }
    }

    public async Task<Edge> GetEdgeAsync(Edge edge)
    {
        try
        {
            await EnsureInitializedAsync();
            if (_state.Projects.TryGetValue(edge.ProjectId.ToString(), out var project))
            {
                var nedge = project.edges?.FirstOrDefault(e => e.Id == edge.Id);
                if (nedge?.Metadata == null)
                {
                    nedge.Metadata = new List<Metadata>();
                }
                return nedge;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get edge {EdgeId}", edge.Id);
            throw;
        }
    }

    public async Task<bool> SyncAsync(Project project)
    {
        try
        {
            await EnsureInitializedAsync();
            var result = await _client.UpsertProject(project);
            if (!result.HasError)
            {
                _state.Projects[project.Id.ToString()] = project;
                return true;
            }

            _logger.LogWarning("Project sync failed: {Error}", result.Errors.First().Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync project {ProjectId}", project.Id);
            throw;
        }
    }

    public async Task<List<SvgFileInfo>> GetIcons()
    {
        try
        {
            return await _client.GetIcons();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get icons");
            throw;
        }
    }
}
