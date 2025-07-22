namespace AlphaPlus.CashService.API.Features.Promotion.Endpoints;

using Microsoft.AspNetCore.Mvc;
using Models;
using Requests;
using Services;
using Shared.Requests;

public static class PromotionEndpoints
{
    public static void MapPromotionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/promotions")
            .WithTags("Promotions");

        group.MapGet("/products", async (string site, int currency, int settingStatus, string language, IPromotionService service) =>
        {
            var products = await service.GetProductAsync(site, currency, settingStatus, language);
            return Results.Ok(products);
        })
        .WithName("GetPromotionProducts")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get promotion products",
            Description = "Returns a tree of available products for promotion"
        });

        group.MapGet("/get", async (int promotionTypeId, int statusId, int custId, short siteDBGroupId, string language, IPromotionService service) =>
        {
            var promotions = await service.GetPromotionAsync(new PromotionRequest { PromotionType = promotionTypeId, StatusId = statusId, ActorId = custId, SiteDBGroupId = siteDBGroupId, Language = language });
            return Results.Ok(promotions);
        })
        .WithName("GetPromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get promotion",
            Description = "Returns a list of available promotion"
        });

        group.MapGet("/{promotionId}", async (int promotionId, int custId, int siteDBGroupId, string language, IPromotionService service) =>
        {
            var promotion = await service.GetPromotionByIdAsync(promotionId, custId, siteDBGroupId, language);
            return Results.Ok(promotion);
        })
        .WithName("GetPromotions")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get promotion detail",
            Description = "Returns a promotion match ID"
        });

        group.MapPost("/", async ([FromBody] PromotionRequest request, IPromotionService service) =>
        {
            var result = await service.AddPromotionAsync(request);
            return Results.Ok(result);
        })
        .WithName("AddPromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add promotion",
            Description = "Add a new promotion"
        });

        group.MapPut("/{promotionId}", async (string promotionId, [FromBody] EditPromotionRequest request, IPromotionService service) =>
        {
            var result = await service.EditPromotionAsync(request);
            return Results.Ok(result);
        })
        .WithName("EditPromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Edit promotion",
            Description = "Edit an existing promotion"
        });

        group.MapPost("/delete/{promotionId}", async (int promotionId, [FromBody] BaseActionRequest request, IPromotionService service) =>
        {
            var result = await service.DeletePromotion(promotionId, request);
            return Results.Ok(result);
        }).WithName("DeletePromotion")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete promotion",
            Description = "Delete a promotion"
        });

        group.MapPost("/addBonus", async ([FromBody] AddBonusRequest request, IPromotionService service) =>
        {
            var result = await service.AddBonusAsync(request);
            return Results.Ok(result);
        }).WithName("AddBonus")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add bonus",
            Description = "Add Bonus"
        });

        group.MapPost("/updateTransactionStatus", async ([FromBody] UpdatePromoteTransactionRequest request, IPromotionService services) =>
        {
            var result = await services.UpdatePromoteTransactionStatusAsync(request);
            return Results.Ok(result);
        }).WithName("UpdateTransactionStatus");

        group.MapGet("/transactions",
            async ([AsParameters] GetPromotionTransactionRequest request, IPromotionService service) =>
            {
                var (promotions, totalCount) = await service.GetPromotionTransactionAsync(request);
                return Results.Ok(new { PromotionTransactions = promotions, TotalCount = totalCount });
            }).WithName("GetPendingPromotions");

        group.MapGet("/reports", async ([AsParameters] GetPromotionReportRequest request, IPromotionReportService service) =>
            Results.Ok(await service.GetMainReport(request))
        ).WithName("PromotionReport");

        group.MapGet("/reports/{id}", async (int id, [AsParameters] GetPromotionDetailsReportRequest request, IPromotionReportService service) =>
            Results.Ok(await service.GetDetailsReport(id, request))
        ).WithName("PromotionDetailsReport");
    }
}