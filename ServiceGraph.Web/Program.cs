using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using ServiceGraph.Web.Components;
using ServiceGraph.Web.Services;
using System.Configuration;

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

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(); // or any type in the assembly
}

builder.Services.AddApplicationInsightsTelemetry();
builder.Logging.AddApplicationInsights();
builder.Logging.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>(
    "", LogLevel.Information);

// Optional: configure logging to also go to Application Insights
//builder.Logging.AddApplicationInsights(
//        configureTelemetryConfiguration: (config) =>
//            config.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"],
//            configureApplicationInsightsLoggerOptions: (options) => {  }
//    );





builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddOutputCache();
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");


var dbSettings = builder.Configuration.GetSection("db").Get<DatabaseSettings>();

if (dbSettings == null)
    throw new Exception("Database settings not found in configuration");


builder.Services.AddSingleton<SvgFileCache>();
builder.Services.AddSingleton(new RepositoryFactory(dbSettings));
builder.Services.AddControllers();
builder.Services.AddAuthorizationCore();
builder.Services.AddFluentUIComponents();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.AddScoped<IServiceClient, ServiceClient>();
builder.Services.AddScoped<ISyncService, SyncService>();
builder.Services.AddScoped<ISyncStateService,SyncStateService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}



app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
    
//app.MapDefaultEndpoints();

app.Run();
