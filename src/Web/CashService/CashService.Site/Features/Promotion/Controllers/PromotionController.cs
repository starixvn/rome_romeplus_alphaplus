namespace AlphaPlus.CashService.Site.Features.Promotion.Controllers;

using Core.Models;
using Fanex.Customer.Client.Services;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Filters;
using AlphaPlus.CashService.Site.Features.Promotion.Models;
using AlphaPlus.CashService.Site.Features.Promotion.Services;

[SiteAuthorizationFilter(PageAccessType.CashPromotion)]
public class PromotionController(
    HttpClientHelper httpClientHelper,
    IConfiguration configuration,
    IUserSession userSession,
    IAntiforgery antiforgery,
    HttpClient httpClient,
    ICustomerService customerService,
    IPromotionReportWorkbook promotionReportWorkbook,
    IPromotionDetailsReportWorkbook promotionDetailsReportWorkbook) : SiteBaseController(userSession, antiforgery, httpClient, customerService)
{
    //private const string CustomerDatelimit = "CustomerDateLimit";
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];
    private readonly IPromotionReportWorkbook promotionReportWorkbook = promotionReportWorkbook;
    private readonly IPromotionDetailsReportWorkbook promotionDetailsReportWorkbook = promotionDetailsReportWorkbook;

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
        return Json(await _httpClient.GetFromJsonAsync<PromotionReport[]>(url));
    }

    public async Task<IActionResult> GetDetailsReport(int promotionId)
    {
        var url = $"{_apiBaseUrl}/promotions/reports/{promotionId}";
        return Json(await _httpClient.GetFromJsonAsync<object>(url));
    }

    public async Task<IActionResult> ExportMainReport()
    {
        var url = $"{_apiBaseUrl}/promotions/reports";
        var reportData = await _httpClient.GetFromJsonAsync<PromotionReport[]>(url);
        var excelFileViewModel = promotionReportWorkbook.BuildExcelFile(reportData);

        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    public async Task<IActionResult> ExportDetailsReport()
    {
        var url = $"{_apiBaseUrl}/promotions/reports/{1}";
        var reportData = await _httpClient.GetFromJsonAsync<PromotionDetailsReport>(url);
        var excelFileViewModel = promotionDetailsReportWorkbook.BuildExcelFile(reportData);

        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    public async Task<IActionResult> GetPromotion(int promotionTypeId, int statusId, CancellationToken cancellationToken)
    {
        var url = $"/promotions/get?promotionTypeId={promotionTypeId}&statusId={statusId}&custId={userSession.CustId}";

        var result = await httpClientHelper.GetAsync<object>(url);

        return Json(result);
    }

    public async Task<IActionResult> GetPromotionById(int promotionId, CancellationToken cancellationToken)
    {
        var url = $"/promotions?promotionId={promotionId}&custId={userSession.CustId}";

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