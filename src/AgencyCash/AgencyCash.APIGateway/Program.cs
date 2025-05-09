using AgencyCash.APIGateway.Shared;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddSingleton(new UMClientConfiguration().Initialize(configuration))
    .AddSingleton<IUMCacheService, UMCacheService>()
    .AddSingleton<IUMRestClient, UMRestClient>()
    .AddSingleton<IMaintenanceService, MaintenanceService>()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddHealthChecks();
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ReverseProxy"));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler();
app.MapHealthChecks("/health");
app.MapReverseProxy(config =>
{
    config
        .UseMiddleware<ApiKeyMiddleware>()
        .UseMiddleware<UMMiddleware>();
});

await app.RunAsync();
