namespace AlphaPlus.CashService.Site.Features.Promotion.Controllers;

using System.Collections.Immutable;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.CashService.Site.Helpers;
using AlphaPlus.Shared.AppResources.CashServices;
using Core.Configurations;
using Core.Models;
using Enum;
using Fanex.Customer.Client.Services;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Products;
using Models.PromotionItems;
using Requests;
using Services;
using Shared;
using Shared.Constants;
using Shared.Filters;
using Shared.Models;
using Shared.Resources;
using Transactions.Enum;
using ViewModels;

[SiteAuthorizationFilter(PageAccessType.CashPromotion)]
public class PromotionController(
    HttpClientHelper httpClientHelper,
    IConfiguration configuration,
    IUserSession userSession,
    IAntiforgery antiforgery,
    HttpClient httpClient,
    ICustomerService customerService,
    IPromotionService promotionService,
    IPromotionReportWorkbook promotionReportWorkbook,
    IPendingPromotionReportWorkbook pendingPromotionReportWorkbook,
    IPromotionDetailsReportWorkbook promotionDetailsReportWorkbook,
    IManualBonusReportWorkbook manualBonusReportWorkbook) : SiteBaseController(userSession, antiforgery, httpClient, customerService)
{
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];

    public async Task<IActionResult> Index()
    {
        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        return View(new SiteBaseViewModel { IsCashServiceEnabled = isCashServiceEnabled });
    }

    public async Task<IActionResult> Pending()
    {
        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        return View(new SiteBaseViewModel { IsCashServiceEnabled = isCashServiceEnabled });
    }

    public IActionResult Report()
    {
        var request = new BaseActionRequest();
        request.SetLimitedDateToShowInfo(userSession);

        return View(request.LimitedDateToShowInfo);
    }

    public async Task<IActionResult> GetMainReport(GetPromotionReportRequest request)
        => Json(await GetPromotionReport(request));

    public async Task<IActionResult> GetDetailsReport(GetPromotionDetailsReportRequest request)
        => Json(await GetPromotionDetailsReport(request));

    public async Task<IActionResult> ExportMainReport(GetPromotionReportRequest request)
    {
        var reportData = await GetPromotionReport(request);
        var excelFileViewModel = promotionReportWorkbook.BuildExcelFile(request, reportData);

        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    public async Task<IActionResult> ExportDetailsReport(GetPromotionDetailsReportRequest request)
    {
        var reportData = await GetPromotionDetailsReport(request);
        var excelFileViewModel = promotionDetailsReportWorkbook.BuildExcelFile(request, reportData);

        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    private Task<PromotionReport[]> GetPromotionReport(GetPromotionReportRequest request)
    {
        request.SetUserSession(userSession);
        if (!request.IsValid())
        {
            return Task.FromResult(Array.Empty<PromotionReport>());
        }

        var url = $"{_apiBaseUrl}/promotions/reports{request.ToQueryString()}";

        return _httpClient.GetFromJsonAsync<PromotionReport[]>(url);
    }

    private Task<PromotionDetailsReport> GetPromotionDetailsReport(GetPromotionDetailsReportRequest request)
    {
        request.SetUserSession(userSession);
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

    public async Task<IActionResult> GetPromotion(int promotionType, int status, CancellationToken cancellationToken)
    {
        var url = $"/promotions/get?promotionTypeId={promotionType}&statusId={status}&custId={userSession.CustId}&sitedbgroupId={userSession.SiteDBGroupId}&language={userSession.Language}";

        var result = await httpClientHelper.GetAsync<List<PromotionDetail>>(url);

        return Json(result);
    }

    public async Task<ImmutableDictionary<string, string>> GetLanguages(CancellationToken cancellationToken)
    {
        var fullLanguages = AppSettings.IsSiteLabel789y ? LanguageConstant.CodeNames : LanguageConstant.CodeKeysWithoutCn3;

        return await Task.FromResult(fullLanguages);
    }

    public async Task<IActionResult> GetPromotionById(int promotionId, CancellationToken cancellationToken)
    {
        var url = $"/promotions/{promotionId}?promotionId={promotionId}&custId={userSession.CustId}&sitedbgroupId={userSession.SiteDBGroupId}&language={userSession.Language}";

        var result = await httpClientHelper.GetAsync<PromotionDetail>(url);

        return Json(result);
    }

    public async Task<IActionResult> GetPromotionProducts()
    {
        var url = $"/promotions/products?site={userSession.Site}&currency={userSession.CurrencyId}&settingstatus=0&language={userSession.Language}";
        var products = await httpClientHelper.GetAsync<List<Product>>(url);
        var productCategoryList = products.GroupBy(p => p.ProductCategoryId).Select(g
            => new ProductDropdown
            {
                ProductCategoryId = g.Key,
                ProductCategoryName = g.First().ProductCategoryName,
                Products = g.ToList()
            });

        return Json(productCategoryList);
    }

    [HttpPost]
    public async Task<IActionResult> AddPromotion([FromForm] AddPromotionRequest request)
    {
        var errMsg = CashServicesResx.InvalidParameters;
        var result = new ResultModel { Code = 1, Message = errMsg };

        if (userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        request.SetUserSession(userSession);
        if (request.IsValid(out errMsg))
        {
            var files = promotionService.ProcessPromotionRequest(request);
            result = await promotionService.ProcessBannerUploadedAsync(files);

            if (result.IsSuccessful)
            {
                CleanUpHtmlContent(request.Contents);
                result = await httpClientHelper.PostAsync<object, ResultModel>("/api/promotions", request);
            }

            return Json(result);
        }

        result.Message = errMsg;

        return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> EditPromotion(int promotionId, [FromForm] EditPromotionRequest request)
    {
        if (userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        request.SetUserSession(userSession);

        var files = promotionService.ProcessEditPromotionRequest(request);
        var result = await promotionService.ProcessBannerUploadedAsync(files);
        if (result.IsSuccessful)
        {
            CleanUpHtmlContent(request.Contents);

            var url = $"/api/promotions/{promotionId}";
            result = await httpClientHelper.PutAsync<EditPromotionRequest, ResultModel>(url, request);
        }

        return Json(result);
    }

    private static void CleanUpHtmlContent(List<PromotionLanguageSetting> listLanguageSettings)
    {
        foreach (var lang in listLanguageSettings)
        {
            var cleanContent = InputCleaner.Clean(lang.Content);
            lang.Content = cleanContent;
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePromotion(int promotionId)
    {
        if (userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var url = $"/api/promotions/delete/{promotionId}";

        var request = new DeletePromotionRequest { PromotionId = promotionId };
        request.SetUserSession(userSession);

        var result = await httpClientHelper.PostAsync<DeletePromotionRequest, ResultModel>(url, request);

        return Json(result);
    }

    public async Task<IActionResult> GetPendingPromotionTransaction(GetPendingPromotionViewModel request)
    {
        request.StatusId = 2;
        request.CustId = userSession.CustId;
        request.SiteDbGroupId = userSession.SiteDBGroupId;
        var result = await httpClientHelper.GetAsync<PromotionTransactionResponse>("/promotions/transactions", ObjectConverter.ToDictionary(request));
        return Json(result);
    }

    public async Task<IActionResult> ExportExcel(GetPendingPromotionViewModel request)
    {
        request.StatusId = 2;
        request.CustId = userSession.CustId;
        request.SiteDbGroupId = userSession.SiteDBGroupId;
        request.Username = userSession.UserName;
        var result = await httpClientHelper.GetAsync<PromotionTransactionResponse>("/promotions/transactions", ObjectConverter.ToDictionary(request));
        var excelFileViewModel = pendingPromotionReportWorkbook.BuildExcelFile(request, result.PromotionTransactions);
        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    public IActionResult GetPromotionTypeOptions()
        => Json(EnumHelper.GetEnumOptions<PromotionType>(CashServicesResx.ResourceManager));

    [HttpPost]
    public async Task<IActionResult> AddBonus([FromBody] AddBonusRequest request)
    {
        if (userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        request.SetUserSession(userSession);
        var url = $"/api/promotions/addBonus";
        var result = await httpClientHelper.PostAsync<AddBonusRequest, ResultModel>(url, request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingAddManualWithdrawalErrCode(result.Code);
        }
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> ApprovePromotionTransaction([FromBody] ProcessPromotionTransactionRequest request)
    {
        if (userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        request.SetUserSession(userSession);
        request.StatusId = (int)TransactionStatus.Completed;
        var result = await UpdatePromoteTransactionStatus(request);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> RejectPromotionTransaction([FromBody] ProcessPromotionTransactionRequest request)
    {
        if (userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        request.SetUserSession(userSession);
        request.StatusId = (int)TransactionStatus.Rejected;
        var result = await UpdatePromoteTransactionStatus(request);
        return Json(result);
    }

    private async Task<ResultModel> UpdatePromoteTransactionStatus(
        [FromBody] ProcessPromotionTransactionRequest request)
    {
        var result = await httpClientHelper.PostAsync<ProcessPromotionTransactionRequest, ResultModel>("/api/promotions/updateTransactionStatus", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingUpdatePromoteTransactionStatusErrCode(result.Code, request.TransId, request.CustId, request.ActorId);
        }
        return result;
    }

    public async Task<IActionResult> GetManualBonusReport(GetManualBonusRequest request)
        => Json(await GetManualBonus(request));

    public async Task<IActionResult> ExportManualBonusReport(GetManualBonusRequest request)
    {
        var reportData = await GetManualBonus(request);
        var excelFileViewModel = manualBonusReportWorkbook.BuildExcelFile(request, reportData);

        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    private Task<ManualBonus[]> GetManualBonus(GetManualBonusRequest request)
    {
        request.SetUserSession(userSession);
        if (!request.IsValid())
        {
            return Task.FromResult(Array.Empty<ManualBonus>());
        }

        var url = $"{_apiBaseUrl}/promotions/reports/ManualBonus{request.ToQueryString()}";

        return _httpClient.GetFromJsonAsync<ManualBonus[]>(url);
    }
}