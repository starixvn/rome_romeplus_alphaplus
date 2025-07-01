using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Services;

namespace AlphaPlus.CashService.API.Features.Promotion.Endpoints;

public static class PromotionEndpoints
{
    public static void MapPromotionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/promotions")
            .WithTags("Promotions");

        group.MapGet("/products", async (IPromotionService service) =>
        {
            var products = await service.GetProductAsync();
            return Results.Ok(products);
        })
        .WithName("GetPromotionProducts")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get promotion products",
            Description = "Returns a tree of available products for promotion"
        });

        group.MapGet("/get", async (int promotionTypeId, int statusId, int custId, IPromotionService service) =>
        {
            var promotions = await service.GetPromotionAsync(promotionTypeId, statusId, custId);
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

        group.MapPost("/", async (PromotionRequest request, IPromotionService service) =>
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

        group.MapPut("/{promotionId}", async (int promotionId, PromotionRequest request, IPromotionService service) =>
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

        group.MapGet("/reports", () =>
        {
            var reports = new[]
            {
                new PromotionReport
                {
                    PromotionId = 1,
                    PromotionTitle = "New Year Sale",
                    PromotionType = "Discount",
                    FromDate = new DateTime(2023, 1, 1),
                    ToDate = new DateTime(2023, 1, 31),
                    TotalRequest = 100,
                    TotalApproved = 80,
                    TotalRejected = 20,
                    TotalBonus = 5000.00m,
                    Status = 1
                },
                new PromotionReport
                {
                    PromotionId = 2,
                    PromotionTitle = "Summer Special",
                    PromotionType = "Cashback",
                    FromDate = new DateTime(2023, 6, 1),
                    ToDate = new DateTime(2023, 6, 30),
                    TotalRequest = 150,
                    TotalApproved = 120,
                    TotalRejected = 30,
                    TotalBonus = 7500.00m,
                    Status = 1
                },
                new PromotionReport
                {
                    PromotionId = 3,
                    PromotionTitle = "Black Friday Deals",
                    PromotionType = "Discount",
                    FromDate = new DateTime(2023, 11, 24),
                    ToDate = new DateTime(2023, 11, 30),
                    TotalRequest = 200,
                    TotalApproved = 180,
                    TotalRejected = 20,
                    TotalBonus = 10000.00m,
                    Status = 1
                },
                new PromotionReport
                {
                    PromotionId = 4,
                    PromotionTitle = "Holiday Bonanza",
                    PromotionType = "Cashback",
                    FromDate = new DateTime(2023, 12, 1),
                    ToDate = new DateTime(2023, 12, 31),
                    TotalRequest = 120,
                    TotalApproved = 100,
                    TotalRejected = 20,
                    TotalBonus = 6000.00m,
                    Status = 1
                },
                new PromotionReport
                {
                    PromotionId = 5,
                    PromotionTitle = "Spring Festival",
                    PromotionType = "Discount",
                    FromDate = new DateTime(2023, 3, 1),
                    ToDate = new DateTime(2023, 3, 31),
                    TotalRequest = 80,
                    TotalApproved = 70,
                    TotalRejected = 10,
                    TotalBonus = 3000.00m,
                    Status = 0
                }
            };
            return TypedResults.Ok(reports);
        }).WithName("PromotionReport");

        group.MapGet("/reports/{id}", (int id) =>
        {
            var details = new[]
            {
                new PromotionDetails
                {
                    Username = "john_doe",
                    Nickname = "john_doe_123",
                    FirstName = "John",
                    LastName = "Doe",
                    PromotionType = "Discount",
                    FromDate = new DateTime(2023, 1, 1),
                    ToDate = new DateTime(2023, 1, 31),
                    BonusAmount = 100.00m,
                    LastUpdated = DateTime.Now,
                    Status = 1
                },
                new PromotionDetails
                {
                    Username = "jane_smith",
                    Nickname = "jane_smith_456",
                    FirstName = "Jane",
                    LastName = "Smith",
                    PromotionType = "Cashback",
                    FromDate = new DateTime(2023, 6, 1),
                    ToDate = new DateTime(2023, 6, 30),
                    BonusAmount = 200.00m,
                    LastUpdated = DateTime.Now,
                    Status = 1
                },
                new PromotionDetails
                {
                    Username = "alice_jones",
                    Nickname = "alice_jones_789",
                    FirstName = "Alice",
                    LastName = "Jones",
                    PromotionType = "Discount",
                    FromDate = new DateTime(2023, 11, 24),
                    ToDate = new DateTime(2023, 11, 30),
                    BonusAmount = 150.00m,
                    LastUpdated = DateTime.Now,
                    Status = 1
                },
                new PromotionDetails
                {
                    Username = "bob_brown",
                    FirstName = "Bob",
                    LastName = "Brown",
                    PromotionType = "Cashback",
                    FromDate = new DateTime(2023, 12, 1),
                    ToDate = new DateTime(2023, 12, 31),
                    BonusAmount = 300.00m,
                    LastUpdated = DateTime.Now,
                    Status = 2
                },
                new PromotionDetails
                {
                    Username = "charlie_white",
                    Nickname = "",
                    FirstName = "Charlie",
                    LastName = "White",
                    PromotionType = "Discount",
                    FromDate = new DateTime(2023, 3, 1),
                    ToDate = new DateTime(2023, 3, 31),
                    BonusAmount = 50.00m,
                    LastUpdated = DateTime.Now,
                    Status = 3
                }
            };
            var reports = new PromotionDetailsReport(
                new PromotionDetailsSummary
                {
                    TotalBonus = details.Sum(d => d.BonusAmount),
                    TotalApproved = details.Count(d => d.Status == 1),
                    TotalRejected = details.Count(d => d.Status == 3)
                },
                details);
            return TypedResults.Ok(reports);
        }).WithName("PromotionDetailsReport");
    }
}