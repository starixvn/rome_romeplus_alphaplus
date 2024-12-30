using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using MediatR;
using Microsoft.Extensions.Options;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Features.Products;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IConsolidationService consolidationService,
    IWinLossDetailService winlossDetailService,
    IConsolidationDetailWorkbook consolidationDetailWorkbook) :
        BaseHandler(productService, options),
        IRequestHandler<ConsolidationDetailQuery, ConsolidationDetailViewModel>,
        IRequestHandler<ConsolidationDetailExcelQuery, ExcelFileViewModel>
{
    private readonly IEnteredResultService enteredResultService = enteredResultService;
    private readonly IConsolidationService consolidationService = consolidationService;
    private readonly IWinLossDetailService winlossDetailService = winlossDetailService;
    private readonly IConsolidationDetailWorkbook consolidationDetailWorkbook = consolidationDetailWorkbook;

    public async Task<ConsolidationDetailViewModel> Handle(ConsolidationDetailQuery message, CancellationToken cancellationToken)
    {
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        var productViewModelTask = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);
        await Task.WhenAll(enteredResultTask, productViewModelTask);

        var (productViewModel, productDetailXml) = await productViewModelTask;
        var reportData = await GetWinLossDetail(message, productViewModel.UserSelectedProductIds, productDetailXml);

        return new ConsolidationDetailViewModel(
            message,
            BuildBreadcrumbViewModel(message),
            productViewModel,
            await enteredResultTask,
            reportData);
    }

    public async Task<ExcelFileViewModel> Handle(ConsolidationDetailExcelQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml, productNames) = await FilterProductAndProductDetailIdsForExcel(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var reportData = await GetWinLossDetail(
            message,
            validUserSelectedProductIds.JoinWithComma(),
            productDetailXml);

        return consolidationDetailWorkbook.BuildExcelFile(message, reportData, productNames);
    }

    private Task<ReportData<WinLossDetailModel>> GetWinLossDetail(
        ConsolidationQuery message,
        string validUserSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossDetailParam(
            message.CustId,
            message.CustLevel.Value,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            Username: null,
            IsReward: false,
            SiteName: message.Site);

        return winlossDetailService.GetWinLossDetail(param);
    }

    private static BreadcrumbViewModel BuildBreadcrumbViewModel(BaseQuery message)
    {
        var breadcrumbs = new List<Breadcrumb>
        {
            new Breadcrumb(message.ViewerId, Resources.Consolidation, message.ViewerLevelId, IsConsolidation: true)
        };

        if (JsonHelper.TryParse<Breadcrumb[]>(message.BreadcrumbsJson, out var breadcrumbList) && breadcrumbList != null)
        {
            for (int i = 1, max = breadcrumbList.Length; i < max; i++)
            {
                if (breadcrumbList[i].CustLevelId == message.CustLevelId)
                {
                    break;
                }

                breadcrumbs.Add(breadcrumbList[i]);
            }
        }

        breadcrumbs.Add(new Breadcrumb(message.CustId, message.CustName, message.CustLevelId));

        return new BreadcrumbViewModel(breadcrumbs, JsonHelper.ToJson(breadcrumbs));
    }

    protected override Task<IEnumerable<Product>> GetProducts(BaseQuery message)
        => consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus, message.Site);
}
