using ServiceGraph.Common;
using System.Text.Json;

namespace ServiceGraph.Web.Services
{
    public class DataSeedingService : IDataSeedingService
    {
        private readonly RepositoryFactory _repositoryFactory;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<DataSeedingService> _logger;

        public DataSeedingService(
            RepositoryFactory repositoryFactory,
            IWebHostEnvironment environment,
            ILogger<DataSeedingService> logger)
        {
            _repositoryFactory = repositoryFactory;
            _environment = environment;
            _logger = logger;
        }

        public async Task SeedSampleDataAsync()
        {
            try
            {
                var projectRepository = _repositoryFactory.CreateRepository<Project>();
                
                // Check if any projects already exist
                var existingProjects = await projectRepository.GetAll();
                if (existingProjects.Any())
                {
                    _logger.LogInformation("Projects already exist in the database. Skipping sample data seeding.");
                    return;
                }

                // Load the sample project from JSON file
                var sampleFilePath = Path.Combine(_environment.ContentRootPath, "sample", "sample.json");
                
                if (!File.Exists(sampleFilePath))
                {
                    _logger.LogWarning("Sample file not found at {SampleFilePath}. Skipping sample data seeding.", sampleFilePath);
                    return;
                }

                var jsonContent = await File.ReadAllTextAsync(sampleFilePath);
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = null // Keep original property names (PascalCase)
                };

                var sampleProject = JsonSerializer.Deserialize<Project>(jsonContent, options);
                
                if (sampleProject == null)
                {
                    _logger.LogError("Failed to deserialize sample project from {SampleFilePath}", sampleFilePath);
                    return;
                }

                // Ensure the project has required properties
                if (sampleProject.Id == Guid.Empty)
                {
                    sampleProject.Id = Guid.NewGuid();
                }

                // Ensure all nodes have the correct ProjectId
                if (sampleProject.nodes != null)
                {
                    foreach (var node in sampleProject.nodes)
                    {
                        if (node.ProjectId == Guid.Empty)
                        {
                            node.ProjectId = sampleProject.Id;
                        }
                    }
                }

                // Ensure all edges have the correct ProjectId
                if (sampleProject.edges != null)
                {
                    foreach (var edge in sampleProject.edges)
                    {
                        if (edge.ProjectId == Guid.Empty)
                        {
                            edge.ProjectId = sampleProject.Id;
                        }
                    }
                }

                // Add the sample project to the repository
                await projectRepository.Add(sampleProject);

                _logger.LogInformation("Successfully seeded sample project '{ProjectName}' with {NodeCount} nodes and {EdgeCount} edges.",
                    sampleProject.ProjectName,
                    sampleProject.nodes?.Count ?? 0,
                    sampleProject.edges?.Count ?? 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed sample data");
            }
        }
    }
}