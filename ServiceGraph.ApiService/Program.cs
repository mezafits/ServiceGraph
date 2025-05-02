 
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .SetBasePath(builder.Environment.ContentRootPath)
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile(
          $"appsettings.{builder.Environment.EnvironmentName}.json",
          optional: true,
          reloadOnChange: true
        ).
        AddEnvironmentVariables();

builder.Services.AddApplicationInsightsTelemetry();

builder.Logging.AddConsole();


// Add service defaults & Aspire components.
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(); // or any type in the assembly
}

builder.Services.AddSingleton<SvgFileCache>();
// Add services to the container.
builder.Services.AddProblemDetails(); 

var adosettings = builder.Configuration.GetSection("FileStorageClientConfig").Get<FileStorageClientConfig>();
var dbSettings = builder.Configuration.GetSection("db").Get<DatabaseSettings>();
 
if (dbSettings == null)
   throw new Exception("Database settings not found in configuration");



builder.Services.AddSingleton<IJsonStorageClient>(new FileStorageClient(adosettings));


builder.Services.AddSingleton<RepositoryFactory>( new RepositoryFactory(dbSettings));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

 
IconServices.ConstructMappings(app);

GraphServices.ConstructMappings(app);

//app.MapDefaultEndpoints();

app.Run();

 
