namespace AlphaPlus.CashService.Site.Features.Promotion.Controllers;

using Core.Models;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Filters;

[SiteAuthorizationFilter(PageAccessType.CashPromotion)]
public class PromotionController(
    HttpClientHelper httpClient,
    IConfiguration configuration,
    IUserSession userSession,
    IAntiforgery antiforgery)
    : SiteBaseController(userSession, antiforgery)
{
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];

    public IActionResult Index() => View();

    public IActionResult Pending() => View();

    public IActionResult Report() => View();

    public async Task<IActionResult> GetPromotion(string promotionType, string status, CancellationToken cancellationToken)
    {
        var url = $"{_apiBaseUrl}/promotion/get-promotion/{promotionType}/{status}";

        var result = await httpClient.GetAsync<object>(url);

        return Json(result);
    }
}