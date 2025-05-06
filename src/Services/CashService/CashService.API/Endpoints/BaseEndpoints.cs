using AlphaPlus.CashService.API.Services;

namespace AlphaPlus.CashService.API.Endpoints;

public static class BaseEndpoints
{
    public static void MapBaseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/base/mobile-prefixes", async (IBaseService service) =>
        {
            var prefixes = await service.GetMobilePrefixes();
            return Results.Ok(prefixes);
        })
        .WithName("GetMobilePrefixes")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get mobile prefixes",
            Description = "Returns a list of all available mobile prefixes"
        });
    }
} 