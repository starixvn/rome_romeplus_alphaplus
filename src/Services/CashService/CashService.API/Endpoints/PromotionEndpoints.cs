using AlphaPlus.CashService.API.Services;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PromotionEndpoints
{
    public static void MapPromotionEndpoints(this WebApplication app)
    {
        app.MapGet("/api/promotion/get-promotion/{type}/{status}", async (string type, string status, IPromotionService service) =>
        {
            var promotions = await service.GetPromotion(type, status);
            return Results.Ok(promotions);
        })
        .WithName("GetPromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get promotion",
            Description = "Returns a list of available promotion"
        });
    }
} 