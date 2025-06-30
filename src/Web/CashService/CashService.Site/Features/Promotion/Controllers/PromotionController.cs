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

    public async Task<IActionResult> GetMainReport()
    {
        var url = $"{_apiBaseUrl}/promotion/reports";
        return Json(await _httpClient.GetFromJsonAsync<object>(url));
    }

    public async Task<IActionResult> GetDetailsReport(int promotionId)
    {
        var url = $"{_apiBaseUrl}/promotion/reports/{promotionId}";
        return Json(await _httpClient.GetFromJsonAsync<object>(url));
    }

    public async Task<IActionResult> GetPromotion(string promotionType, string status, CancellationToken cancellationToken)
    {
        var url = $"{_apiBaseUrl}/promotion/get-promotion/{promotionType}/{status}";

        var result = await httpClientHelper.GetAsync<object>(url);

        return Json(result);
    }
}