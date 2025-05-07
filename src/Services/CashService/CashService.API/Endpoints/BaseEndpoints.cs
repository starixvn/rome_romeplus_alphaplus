using AlphaPlus.CashService.API.Services;

namespace AlphaPlus.CashService.API.Endpoints;

public static class BaseEndpoints
{
    public static void MapBaseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/base/country-codes/{custId}", async (int custId, IBaseService service) =>
        {
            var prefixes = await service.GetCountryCodes(custId);
            return Results.Ok(prefixes);
        })
        .WithName("GetCountryCodes")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get country codes",
            Description = "Returns a list of all available country codes"
        });
    }
} 