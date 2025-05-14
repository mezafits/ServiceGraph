using ServiceGraph.Common;

public interface ISyncStateService
{
    Dictionary<string, Project> Projects { get; }
    Guid SelectedProjectId { get; set; }
    Project SelectedProject { get; }
    Task InitializeAsync(string userName, IServiceClient serviceClient);
    Task RefreshProjectsAsync(string userName, IServiceClient serviceClient);
}
