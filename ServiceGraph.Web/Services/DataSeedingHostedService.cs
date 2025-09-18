using ServiceGraph.Common;
using System.Text.Json;

namespace ServiceGraph.Web.Services
{
    public class DataSeedingHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DataSeedingHostedService> _logger;
        private readonly IConfiguration _configuration;

        public DataSeedingHostedService(
            IServiceProvider serviceProvider,
            ILogger<DataSeedingHostedService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var dbSettings = _configuration.GetSection("db").Get<DatabaseSettings>();
            
            if (dbSettings == null)
            {
                _logger.LogWarning("Database settings not found in configuration. Skipping sample data seeding.");
                return;
            }
            
            if (!dbSettings.UseInMemoryDatabase)
            {
                _logger.LogInformation("Not using in-memory database. Skipping sample data seeding.");
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var repositoryFactory = scope.ServiceProvider.GetRequiredService<RepositoryFactory>();
                var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                var projectRepository = repositoryFactory.CreateRepository<Project>();

                // Check if any projects already exist
                var existingProjects = await projectRepository.GetAll();
                _logger.LogInformation("Found {ExistingProjectCount} existing projects in database", existingProjects.Count());
                
                if (existingProjects.Any())
                {
                    foreach (var project in existingProjects)
                    {
                        _logger.LogInformation("Existing project: '{ProjectName}' (ID: {ProjectId}, IsPublic: {IsPublic})", 
                            project.ProjectName, project.Id, project.IsPublic);
                    }
                    _logger.LogInformation("Projects already exist in the database. Skipping sample data seeding.");
                    return;
                }

                // Load the sample project from JSON file
                var sampleFilePath = Path.Combine(environment.ContentRootPath, "sample", "sample.json");

                if (!File.Exists(sampleFilePath))
                {
                    _logger.LogWarning("Sample file not found at {SampleFilePath}. Skipping sample data seeding.", sampleFilePath);
                    return;
                }

                var jsonContent = await File.ReadAllTextAsync(sampleFilePath, cancellationToken);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = null
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

                // Make the project public so it's visible to all users
                sampleProject.IsPublic = true;

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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}