namespace AlphaPlus.CashService.API.Endpoints;

public static class SystemEndpoints
{
    public static void MapSystemEndpoints(this WebApplication app)
    {
        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
            .WithName("HealthCheck")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Health check endpoint",
                Description = "Returns the health status of the API"
            });

        app.MapGet("/version", () => Results.Ok(new { Version = "1.0.0", Environment = app.Environment.EnvironmentName }))
            .WithName("Version")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Version information",
                Description = "Returns the current version of the API"
            });
    }
}