using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using AlphaPlus.CashService.API.Features.Promotion.Services;

namespace AlphaPlus.CashService.API.Features.Promotion.Endpoints;

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

        group.MapPost("/{promotionId}", async (int promotionId, [FromBody] BaseActionRequest request, IPromotionService service) =>
        {
            var result = await service.DeletePromotion(promotionId, request);
            return Results.Ok(result);
        });
        group.MapGet("/reports", ([AsParameters] GetPromotionReportRequest request, IPromotionService service) =>
        {
            return TypedResults.Ok(FakeReport());
        }).WithName("PromotionReport");

        group.MapGet("/reports/{id}", (int id, [AsParameters] GetPromotionDetailsReportRequest request, IPromotionService service) =>
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

    private static IEnumerable<PromotionReport> FakeReport()
    {
        var reports = new List<PromotionReport>();
        var random = new Random();
        var promotionTypes = new[] { "Discount", "Cashback", "Bonus", "Rebate", "Loyalty" };
        var titles = new[] { "Sale", "Special", "Deal", "Promotion", "Offer", "Bonanza", "Festival", "Extravaganza", "Event", "Campaign" };
        var prefixes = new[] { "Holiday", "Summer", "Winter", "Spring", "Fall", "New Year", "Christmas", "Easter", "Black Friday", "Cyber Monday", "Weekend", "Monthly", "Annual", "Seasonal", "Limited Time" };
        var statuses = new[] { false, true, true, true, true, false, false }; // Weighted to have more status 1 items

        for (int i = 1; i <= 100; i++)
        {
            // Generate random dates within 2023
            var fromMonth = random.Next(1, 12);
            var fromDay = random.Next(1, 28);
            var durationDays = random.Next(7, 31);
            var fromDate = new DateTime(2023, fromMonth, fromDay);
            var toDate = fromDate.AddDays(durationDays);

            var totalRequest = random.Next(50, 300);
            var totalApproved = random.Next(30, totalRequest);
            var totalRejected = totalRequest - totalApproved;

            reports.Add(new PromotionReport
            {
                PromotionId = i,
                PromotionTitle = $"{prefixes[random.Next(prefixes.Length)]} {titles[random.Next(titles.Length)]} {random.Next(1, 10)}",
                PromotionType = promotionTypes[random.Next(promotionTypes.Length)],
                FromDate = fromDate,
                ToDate = toDate,
                TotalRequest = totalRequest,
                TotalApproved = totalApproved,
                TotalRejected = totalRejected,
                TotalBonus = Math.Round(random.Next(1000, 15000) * 1.0m, 2),
                Status = statuses[random.Next(statuses.Length)]
            });
        }

        return reports;
    }
}