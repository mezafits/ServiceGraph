using ServiceGraph.Common;

public interface ISyncService
{
    Task<Project> GetSelectedProjectAsync();
    Task SetSelectedProjectAsync(string projectId);
    Task UpsertProjectAsync(Project project);
    Task UpsertServiceNodeAsync(ServiceNode node);
    Task UpsertEdgeAsync(Edge edge);
    Task RemoveServiceNodeAsync(ServiceNode node);

    Task RemoveServiceNodeFromGroupAsync(ServiceNode node);
    Task RemoveEdgeAsync(Edge edge);
    Task<ServiceNode> AddServiceNodeToGroupAsync(Guid projectId, Guid parentId, Guid serviceNodeId);
    Task<ServiceNode> GetServiceNodeAsync(ServiceNode node);
    Task<ServiceNode> GetServiceNodeByIdAsync(Guid projectId, Guid serviceNodeId);
    Task<Edge> GetEdgeAsync(Edge edge);
    Task<List<Project>> GetProjectsAsync();
    Task<bool> SyncAsync(Project project);
    //Task<List<SvgFileInfo>> GetIcons();
}
