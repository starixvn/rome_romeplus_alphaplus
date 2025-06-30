namespace AlphaPlus.CashService.Site.Features.Promotion.Controllers;

using Core.Models;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Filters;
using Fanex.Customer.Client.Services;

[SiteAuthorizationFilter(PageAccessType.CashPromotion)]
public class PromotionController(
    HttpClientHelper httpClientHelper,
    IConfiguration configuration,
    IUserSession userSession,
    IAntiforgery antiforgery,
    HttpClient httpClient,
    ICustomerService customerService) : SiteBaseController(userSession, antiforgery, httpClient, customerService)
{
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];

    public IActionResult Index() => View();

    public IActionResult Pending() => View();

    public IActionResult Report() => View();

    public IActionResult GetPromotionReport()
    {
        var fakeData = new object[]
        {
            new
            {
                promotionId = 1,
                promotionTitle = "New Year Sale",
                promotionType = "Discount",
                period = "01 Jan 2023 - 31 Jan 2023",
                totalRequest = 100,
                totalApproved = 80,
                totalRejected = 20,
                totalBonus = 5000,
                status = "Approved",
            },
            new
            {
                promotionId = 2,
                promotionTitle = "Summer Sale",
                promotionType = "Cashback",
                period = "01 Jun 2023 - 30 Jun 2023",
                totalRequest = 150,
                totalApproved = 120,
                totalRejected = 30,
                totalBonus = 7500,
                status = "Pending",
            },
            new
            {
                promotionId = 3,
                promotionTitle = "Black Friday",
                promotionType = "Discount",
                period = "01 Nov 2023 - 30 Nov 2023",
                totalRequest = 200,
                totalApproved = 180,
                totalRejected = 20,
                totalBonus = 10000,
                status = "Reject",
            }
        };

        return Json(fakeData);
    }

    public IActionResult GetPromotionDetailsReport()
    {
        var fakeData = new object[]
        {
            new
            {
                username = "user2",
                firstName = "Jane",
                lastName = "Doe",
                promotionType = "Summer Sale",
                dateRange = "01 Jun 2023 - 30 Jun 2023",
                bonusAmount = 120,
                lastUpdated = "30 Jun 2023 15:45:59",
                status = "Pending",
            },
            new
            {
                username = "user3",
                firstName = "Alice",
                lastName = "Smith",
                promotionType = "Black Friday",
                dateRange = "01 Nov 2023 - 30 Nov 2023",
                bonusAmount = 180,
                lastUpdated = "05 Nov 2023 10:30:00",
                status = "Reject",
            },
            new
            {
                username = "user4",
                firstName = "Bob",
                lastName = "Johnson",
                promotionType = "New Year Sale",
                dateRange = "01 Jan 2023 - 31 Jan 2023",
                bonusAmount = 80,
                lastUpdated = "02 Jan 2023 12:00:00",
                status = "Approved",
            }
        };

        return Json(fakeData);
    }

    public async Task<IActionResult> GetPromotion(string promotionType, string status, CancellationToken cancellationToken)
    {
        var url = $"{_apiBaseUrl}/promotion/get-promotion/{promotionType}/{status}";

        var result = await httpClientHelper.GetAsync<object>(url);

        return Json(result);
    }
}