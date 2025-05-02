


using Microsoft.VisualStudio.Services.Common;
using ServiceGraph.Common;

public static class GraphServices
    {
    
    public static void ConstructMappings(WebApplication app)
        {

        app.MapPut("/Project", async (Project projectRequest, RepositoryFactory repo) =>
        {
            var logger = app.Logger; // Get the logger from the app instance
            logger.LogInformation("Handling PUT request for /UpdateNode");

            try
            {
                var serviceNodeRepo = repo.CreateRepository<Project>();
                var validationResults = Validator.ValidateProject(projectRequest);

                if (validationResults.Any())
                {
                    logger.LogWarning("Validation errors occurred in /UpdateNode");
                    return Results.Ok(new { HasErrors = true, Errors = validationResults });
                }

                await serviceNodeRepo.CreateOrUpdate(projectRequest);
                logger.LogInformation("PUT request for /UpdateNode completed successfully");
                return Results.Ok(new { HasErrors = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during PUT request for /UpdateNode");
                return Results.Problem($"An error occurred: {ex.Message}");
            }
        });

        app.MapGet("/Projects/{user}", async (string user, RepositoryFactory repo, ILogger<Program> logger) =>
        {
            try
            {
                List<Project> projects = new List<Project>();
                var projectsRepo = repo.CreateRepository<Project>();
                var results = await projectsRepo.Query(x => x.Owners.Contains(user) || x.Readers.Contains(user));
                logger.LogTrace($"Found {results.Count()} projects for user {user}.");    
                if (results.Any())
                {
                    projects.AddRange(results);
                }

                if (!projects.Any())
                {
                    var sampleData = ProjectUtilities.GenerateDefaultProject(user);
                    await projectsRepo.CreateOrUpdate(sampleData);
                    projects.Add(sampleData);
                }
                logger.LogTrace($"Returning {projects.Count} projects for user {user}.");
                return projects;
            }
            catch (Exception err)
            {
                logger.LogError(err, "Error generating sample data");
                throw;
            }
        });
       
        app.MapPost("/Import", async (HttpContext context, RepositoryFactory repo) =>
            {
                try
                {
                    // Assuming the JSON file is sent in the request body
                    var project = await context.Request.ReadFromJsonAsync<Project>();

                    if (project == null)
                    {
                        context.Response.StatusCode = 400; // Bad Request
                        await context.Response.WriteAsync("Invalid data format.");
                        return;
                    }
                    var projectRepo = repo.CreateRepository<Project>();
                    
                    await projectRepo.CreateOrUpdate(project);
                 
                    await context.Response.WriteAsync("Data imported successfully.");
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error, return an error response)
                    context.Response.StatusCode = 500; // Internal Server Error
                    await context.Response.WriteAsync("An error occurred: " + ex.Message);
                }
            });

        app.MapGet("/Export", async (string ProjectId,HttpContext context, RepositoryFactory repo, IJsonStorageClient doc) =>
            {
                try
                {
                    var projectRepo = repo.CreateRepository<Project>();

                    var id = Guid.Parse(ProjectId);
                    var project = await projectRepo.GetById(id, id);

                    var contentType = "application/json";
                    context.Response.ContentType = contentType;
                    context.Response.Headers.Add("Content-Disposition", "attachment; filename=export.json");
                     
                    await context.Response.WriteAsJsonAsync(project);
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error, return an error response)
                    context.Response.StatusCode = 500; // Internal Server Error
                    await context.Response.WriteAsync("An error occurred: " + ex.Message);
                }
            });

        }
    }
 
