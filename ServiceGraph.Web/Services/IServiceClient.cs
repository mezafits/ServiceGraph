// The intention of this code is to provide a service for managing graph projects. which will replace the rest endpoint
using ServiceGraph.Common;

public interface IServiceClient
{
    Task<OperationResult> UpsertProject(Project projectRequest);
    Task<List<Project>> GetProjectsAsync(string user);
    Task<OperationResult> ImportProjectAsync(Project project);
    Task<Project?> ExportProjectAsync(Guid projectId);
}
