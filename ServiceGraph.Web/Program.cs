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
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

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
// Add service defaults & Aspire components.

var serviceBaseAddress = builder.Configuration
                               .GetValue<string>("ServiceClient:BaseAddress");
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddScoped<IKustoQueryService, KustoQueryService>();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddOutputCache();

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");
    //.EnableTokenAcquisitionToCallDownstreamApi()
    //.AddInMemoryTokenCaches();

builder.Services.AddControllers();
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpClient<ServiceClient>(client=> client.BaseAddress = new(serviceBaseAddress));
builder.Services.AddFluentUIComponents();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();
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
