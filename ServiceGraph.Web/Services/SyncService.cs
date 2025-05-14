using Microsoft.AspNetCore.Components.Authorization;
using ServiceGraph.Common;

// scoped service
public class SyncService : ISyncService
{
    private readonly ISyncStateService _state;
    private readonly IHttpClientFactory _clientFactory;
    private readonly AuthenticationStateProvider _authProvider;
    private readonly ILogger<SyncService> _logger;
    private readonly RepositoryFactory _repoFactory;

    private IServiceClient _client;
    private string _userName;

    public SyncService(
        ISyncStateService state,
        AuthenticationStateProvider authProvider,
        ILogger<SyncService> logger,
        IServiceClient client)
    {
        _state = state;
        _authProvider = authProvider;
        _logger = logger;
        _client = client;
    }

    private async Task EnsureInitializedAsync()
    {
        _logger.LogInformation("Calling EnsureInitializedAsync");

        try
        {
            //if (_client != null)
            //{
            //    _logger.LogInformation("_client already initialized. Skipping initialization.");
            //    return;
            //}

            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            _logger.LogInformation("User Identity IsAuthenticated: {IsAuthenticated}", user.Identity?.IsAuthenticated);
            _logger.LogInformation("User Identity Name: {Name}", user.Identity?.Name ?? "null");

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                throw new InvalidOperationException("User is not authenticated");
            }

            //// Log all claims
            //foreach (var claim in user.Claims)
            //{
            //    _logger.LogInformation("Claim Type: {ClaimType}, Value: {ClaimValue}", claim.Type, claim.Value);
            //}

            // Attempt to determine _userName
            _userName = user.Identity.Name
                ?? user.Claims.FirstOrDefault(c => c.Type.Contains("email", StringComparison.OrdinalIgnoreCase))?.Value
                ?? throw new InvalidOperationException("Unable to determine user identity");

            if (string.IsNullOrEmpty(_userName))
            {
                throw new InvalidOperationException("User name is null or empty");
            }

            _logger.LogInformation("User name resolved as: {UserName}", _userName);

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
            return null;
            //throw;
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
            if (result.HasErrors)
            {
                _logger.LogError("Error upserting project {ProjectId}: {Error}", project.Id, result.Errors.First());
                throw new InvalidOperationException(result.Errors.First());
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
            if (!result.HasErrors)
            {
                _state.Projects[project.Id.ToString()] = project;
                return true;
            }

            _logger.LogWarning("Project sync failed: {Error}", result.Errors.First());
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync project {ProjectId}", project.Id);
            throw;
        }
    }

    //public async Task<List<SvgFileInfo>> GetIcons()
    //{
    //    try
    //    {
    //        return await _client.GetIcons();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Failed to get icons");
    //        throw;
    //    }
    //}
}
