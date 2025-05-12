using AlphaPlus.CashService.API.Services;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PromotionEndpoints
{
    public static void MapPromotionEndpoints(this WebApplication app)
    {
        app.MapGet("/api/promotion/get-promotion/{promotionType}/{status}", async (string promotionType, string status, IPromotionService service) =>
        {
            var promotions = await service.GetPromotion(promotionType, status);
            return Results.Ok(promotions);
        })
        .WithName("GetPromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get promotion",
            Description = "Returns a list of available promotion"
        });

        app.MapDelete("/api/promotion/delete/{custId:int}/{promotionId:int}", async (int custId, int promotionId, IPromotionService service) =>
        {
            var result = await service.DeletePromotion(custId, promotionId);
            return Results.Ok(result);
        })
        .WithName("DeletePromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete promotion",
            Description = "Deletes a promotion by its ID for a specific customer."
        });
    }
} 