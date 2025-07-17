namespace AlphaPlus.CashService.API.Features.Promotion.Endpoints;

using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using AlphaPlus.CashService.API.Features.Promotion.Services;
using Microsoft.AspNetCore.Mvc;
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

        group.MapGet("/get", async (int promotionTypeId, int statusId, int custId, int siteDBGroupId, string language, IPromotionService service) =>
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

        group.MapGet("/", async (int promotionId, int custId, IPromotionService service) =>
        {
            var promotion = await service.GetPromotionByIdAsync(promotionId, custId);
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

        group.MapPut("/{promotionId}", async (int promotionId, [FromBody] PromotionRequest request, IPromotionService service) =>
        {
            var result = await service.EditPromotion(promotionId, request);
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
        group.MapGet("/reports", async ([AsParameters] GetPromotionReportRequest request, IPromotionReportService service) =>
            Results.Ok(await service.GetMainReport(request))
        ).WithName("PromotionReport");

        group.MapGet("/reports/{id}", async (int id, [AsParameters] GetPromotionDetailsReportRequest request, IPromotionReportService service) =>
            Results.Ok(await service.GetDetailsReport(id, request))
        ).WithName("PromotionDetailsReport");
    }
}