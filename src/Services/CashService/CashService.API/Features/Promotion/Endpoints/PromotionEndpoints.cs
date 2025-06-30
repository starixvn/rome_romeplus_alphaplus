using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Services;

namespace AlphaPlus.CashService.API.Features.Promotion.Endpoints;

public static class PromotionEndpoints
{
    public static void MapPromotionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/promotion").WithTags("Promotion");

        group.MapGet("/get-promotion/{promotionType}/{status}", async (string promotionType, string status, IPromotionService service) =>
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

        group.MapDelete("/delete/{custId:int}/{promotionId:int}", async (int custId, int promotionId, IPromotionService service) =>
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
                    Nickname = "John",
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
                    Nickname = "Jane",
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
                    Nickname = "Alice",
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
                    Nickname = "Bob",
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
                    Nickname = "Charlie",
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