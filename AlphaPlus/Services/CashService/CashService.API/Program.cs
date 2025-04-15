using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CashService API",
        Version = "v1",
        Description = "A minimal API for handling cash-related operations",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@cashservice.com"
        }
    });
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy => policy
            .WithOrigins("http://localhost:3000") // Add your frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashService API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var result = JsonSerializer.Serialize(new
        {
            error = exception?.Message ?? "An error occurred",
            statusCode = StatusCodes.Status500InternalServerError
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(result);
    });
});

app.UseCors("AllowSpecificOrigins");

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi(operation => new(operation)
    {
        Summary = "Health check endpoint",
        Description = "Returns the health status of the API"
    });

// Version endpoint
app.MapGet("/version", () => Results.Ok(new { Version = "1.0.0", Environment = app.Environment.EnvironmentName }))
    .WithName("Version")
    .WithOpenApi(operation => new(operation)
    {
        Summary = "Version information",
        Description = "Returns the current version of the API"
    });

app.Run();
