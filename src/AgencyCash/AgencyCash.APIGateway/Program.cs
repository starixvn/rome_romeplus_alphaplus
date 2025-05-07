using AgencyCash.APIGateway.DelegatingHandlers;
using AgencyCash.APIGateway.Shared;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
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
    .AddOcelot(configuration.GetSection("Ocelot"))
    .AddPolly()
    .AddCacheManager(x => x.WithDictionaryHandle())
    .AddDelegatingHandler<ApiKeyHandler>(true)
    .AddDelegatingHandler<UMHandler>(true)
    .AddDelegatingHandler<NoGzipDelegatingHandler>(true);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}

app
    .UseRouting()
    .UseEndpoints(config => config.MapHealthChecks("/health"))
    .UseOcelot()
    .Wait();

await app.RunAsync();
