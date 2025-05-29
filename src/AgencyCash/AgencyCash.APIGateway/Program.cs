using System.Globalization;
using System.Text.Json;
using System.Threading.RateLimiting;
using AgencyCash.APIGateway.Shared;
using Microsoft.AspNetCore.RateLimiting;
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

var rateLimitConfig = configuration.GetSection("RateLimiting:FixedWindow");
builder.Services.AddRateLimiter(options =>
{
    options
        .AddFixedWindowLimiter(policyName: "FixedWindow", opt =>
        {
            opt.PermitLimit = rateLimitConfig.GetValue<int>("PermitLimit");
            opt.Window = TimeSpan.FromSeconds(rateLimitConfig.GetValue<int>("WindowSeconds"));
        })
        .OnRejected = async (context, cancellationToken) =>
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
            }

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                code = StatusCodes.Status429TooManyRequests,
                message = "Too many requests. Please try again later."
            }), cancellationToken);
        };
});

builder.Services
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "API Gateway", Version = "v1" });
        c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "API Key needed to access the endpoints. ApiKey: X-API-KEY: {key}",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Name = "X-API-KEY",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "ApiKey"
        });
        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    Scheme = "ApiKey",
                    Name = "X-API-KEY",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    })
    .AddEndpointsApiExplorer();

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
        .UseMiddleware<UMMiddleware>()
        .UseRateLimiter();
});

// Register Gateway Swagger UI at a unique path to avoid conflict with proxy
app
    .UseSwagger(c => c.RouteTemplate = "gateway-swagger/{documentName}/swagger.json")
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/gateway-swagger/v1/swagger.json", "API Gateway");
        c.RoutePrefix = ""; // Serve Gateway Swagger UI at root
    });

await app.RunAsync();
