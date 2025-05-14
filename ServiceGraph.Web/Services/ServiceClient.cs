// The intention of this code is to provide a service for managing graph projects. which will replace the rest endpoint
using Microsoft.Extensions.Logging;
using ServiceGraph.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ServiceClient : IServiceClient
{
    private readonly RepositoryFactory _repoFactory;
    private readonly ILogger<ServiceClient> _logger;

    public ServiceClient(
        RepositoryFactory repoFactory,
        ILogger<ServiceClient> logger)
    {
        _repoFactory = repoFactory;
        _logger = logger;
    }

    public async Task<OperationResult> UpsertProject(Project projectRequest)
    {
        _logger.LogInformation("Starting SaveOrUpdateProjectAsync");

        var validationResults = Validator.ValidateProject(projectRequest);
        if (validationResults.Any())
        {
            _logger.LogWarning("Validation errors occurred in SaveOrUpdateProjectAsync");
            return new OperationResult
            {
                HasErrors = true,
                Errors = validationResults.Select(v => v.Message).ToList()
            };
        }

        try
        {
            var serviceNodeRepo = _repoFactory.CreateRepository<Project>();
            await serviceNodeRepo.CreateOrUpdate(projectRequest);

            _logger.LogInformation("Project saved or updated successfully.");
            return new OperationResult { HasErrors = false };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in SaveOrUpdateProjectAsync");
            return new OperationResult
            {
                HasErrors = true,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<List<Project>> GetProjectsAsync(string user)
    {
        _logger.LogInformation($"Fetching projects for user: {user}");

        var projectsRepo = _repoFactory.CreateRepository<Project>();
        var results = await projectsRepo.Query(x => x.Owners.Contains(user) || x.Readers.Contains(user) || x.IsPublic);

        if (!results.Any())
        {
            _logger.LogInformation($"No projects found for user {user}, generating default.");
            var sampleData = ProjectUtilities.GenerateDefaultProject(user);
            await projectsRepo.CreateOrUpdate(sampleData);
            return new List<Project> { sampleData };
        }

        return results.ToList();
    }

    public async Task<OperationResult> ImportProjectAsync(Project project)
    {
        _logger.LogInformation("Importing project data");

        try
        {
            var projectRepo = _repoFactory.CreateRepository<Project>();
            await projectRepo.CreateOrUpdate(project);
            return new OperationResult { HasErrors = false };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during import");
            return new OperationResult
            {
                HasErrors = true,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<Project?> ExportProjectAsync(Guid projectId)
    {
        _logger.LogInformation($"Exporting project with ID: {projectId}");

        try
        {
            var projectRepo = _repoFactory.CreateRepository<Project>();
            return await projectRepo.GetById(projectId, projectId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during export");
            return null;
        }
    }
}

public class OperationResult
{
    public bool HasErrors { get; set; }
    public List<string> Errors { get; set; } = new();
}
