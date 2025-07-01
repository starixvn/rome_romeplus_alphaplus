namespace AlphaPlus.CashService.Site.Features.Promotion.Controllers;

using Core.Models;
using Fanex.Customer.Client.Services;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Filters;

[SiteAuthorizationFilter(PageAccessType.CashPromotion)]
public class PromotionController(
    HttpClientHelper httpClientHelper,
    IConfiguration configuration,
    IUserSession userSession,
    IAntiforgery antiforgery,
    HttpClient httpClient,
    ICustomerService customerService) : SiteBaseController(userSession, antiforgery, httpClient, customerService)
{
    //private const string CustomerDatelimit = "CustomerDateLimit";
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];

    public IActionResult Index() => View();

    public IActionResult Pending() => View();

    public IActionResult Report()
    {
        // var today = SystemTime.Today;
        // var minDate = new DateTime(today.Year - 3, 1, 1);
        // var limitedDateToShowInfo = userSession.GetProperty<DateTime>(CustomerDatelimit);
        // var viewModel = new PromotionReportViewModel(minDate.GetGreaterDate(limitedDateToShowInfo), today);
        return View();
    }

    public async Task<IActionResult> GetMainReport()
    {
        var url = $"{_apiBaseUrl}/promotions/reports";
        return Json(await _httpClient.GetFromJsonAsync<object>(url));
    }

    public async Task<IActionResult> GetDetailsReport(int promotionId)
    {
        var url = $"{_apiBaseUrl}/promotions/reports/{promotionId}";
        return Json(await _httpClient.GetFromJsonAsync<object>(url));
    }

    public async Task<IActionResult> GetPromotion(int promotionTypeId, int statusId, CancellationToken cancellationToken)
    {
        var url = $"/promotions/get?promotionTypeId={promotionTypeId}&statusId={statusId}&custId={userSession.CustId}";

        var result = await httpClientHelper.GetAsync<object>(url);

        return Json(result);
    }

    public async Task<IActionResult> GetPromotionProducts()
    {
        var url = "/promotions/products";
        var result = await httpClientHelper.GetAsync<object>(url);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddPromotion([FromBody] object request)
    {
        var url = $"/api/promotions";
        var result = await httpClientHelper.PostAsync<object, object>(url, request);
        return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> EditPromotion(int promotionId, [FromBody] object request)
    {
        var url = $"/api/promotions/{promotionId}";
        await httpClientHelper.PutAsync<object>(url, request);
        return Json(new { success = true });
    }
}