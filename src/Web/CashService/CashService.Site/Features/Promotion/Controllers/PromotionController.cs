namespace AlphaPlus.CashService.Site.Features.Promotion.Controllers;

using AlphaPlus.CashService.Site.Features.Promotion.Requests;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using Core.Models;
using Fanex.Customer.Client.Services;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Products;
using Services;
using Shared;
using Shared.Filters;
using Shared.Models;

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
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];

    public IActionResult Index() => View();

    public IActionResult Pending() => View();

    public IActionResult Report()
    {
        var request = new BaseActionRequest()
            .SetLimitedDateToShowInfo(userSession);

        return View(request.LimitedDateToShowInfo);
    }

    public async Task<IActionResult> GetMainReport(GetPromotionReportRequest request)
        => Json(await GetPromotionReport(request));

    public async Task<IActionResult> GetDetailsReport(GetPromotionDetailsReportRequest request)
        => Json(await GetPromotionDetailsReport(request));

    public async Task<IActionResult> ExportMainReport(GetPromotionReportRequest request)
    {
        var reportData = await GetPromotionReport(request);
        var excelFileViewModel = promotionReportWorkbook.BuildExcelFile(reportData);

        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    public async Task<IActionResult> ExportDetailsReport(GetPromotionDetailsReportRequest request)
    {
        var reportData = await GetPromotionDetailsReport(request);
        var excelFileViewModel = promotionDetailsReportWorkbook.BuildExcelFile(reportData);

        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    private Task<PromotionReport[]> GetPromotionReport(GetPromotionReportRequest request)
    {
        request
            .SetUserSession(userSession)
            .SetLimitedDateToShowInfo(userSession);
        if (!request.IsValid())
        {
            return Task.FromResult(Array.Empty<PromotionReport>());
        }

        var url = $"{_apiBaseUrl}/promotions/reports{request.ToQueryString()}";

        return _httpClient.GetFromJsonAsync<PromotionReport[]>(url);
    }

    private Task<PromotionDetailsReport> GetPromotionDetailsReport(GetPromotionDetailsReportRequest request)
    {
        request
            .SetUserSession(userSession)
            .SetLimitedDateToShowInfo(userSession);
        if (!request.IsValid())
        {
            return Task.FromResult(new PromotionDetailsReport(
                new PromotionDetailsSummary(),
                Array.Empty<PromotionDetails>()
            ));
        }

        var url = $"{_apiBaseUrl}/promotions/reports/{request.PromotionId}{request.ToQueryString()}";
        return _httpClient.GetFromJsonAsync<PromotionDetailsReport>(url);
    }

    public async Task<IActionResult> GetPromotion(int promotionTypeId, int statusId, CancellationToken cancellationToken)
    {
        var url = $"/promotions/get?promotionTypeId={promotionTypeId}&statusId={statusId}&custId={userSession.CustId}&sitedbgroupId={userSession.SiteDBGroupId}";

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
        var products = await httpClientHelper.GetAsync<List<Product>>(url);
        var productCategoryList = products.GroupBy(p => p.ProductCategoryId).Select(g
            => new ProductDropdown
            {
                ProductCategoryId = g.Key,
                Products = g.ToList()
            });

        return Json(productCategoryList);
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

    [HttpDelete]
    public async Task<IActionResult> DeletePromotion(int promotionId)
    {
        var url = $"/api/promotions/delete/{promotionId}";

        var request = new DeletePromotionRequest { PromotionId = promotionId };
        var result = await httpClientHelper.PostAsync<DeletePromotionRequest, ResultModel>(url, request);
        return Json(result);
    }
}