using AlphaPlus.CashService.API.Services;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PromotionEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/promotion/get-promotion", async (IPromotionService service) =>
        {
            var providers = await service.GetPromotion();
            return Results.Ok(providers);
        })
        .WithName("GetPromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get promotion",
            Description = "Returns a list of available promotion"
        });
    }
} 