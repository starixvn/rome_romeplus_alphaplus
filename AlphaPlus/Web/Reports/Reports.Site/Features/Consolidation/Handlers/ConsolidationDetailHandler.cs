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
using AlphaPlus.Reports.Site.Features.WinLossDetail;
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
    IWinLossDetailWorkbook winLossDetailWorkbook) :
        BaseHandler(productService, options),
        IRequestHandler<ConsolidationDetailQuery, ConsolidationDetailViewModel>,
        IRequestHandler<ConsolidationDetailExcelQuery, ExcelFileViewModel>
{
    private readonly IEnteredResultService enteredResultService = enteredResultService;
    private readonly IConsolidationService consolidationService = consolidationService;
    private readonly IWinLossDetailService winlossDetailService = winlossDetailService;
    private readonly IWinLossDetailWorkbook winLossDetailWorkbook = winLossDetailWorkbook;

    public async Task<ConsolidationDetailViewModel> Handle(ConsolidationDetailQuery message, CancellationToken cancellationToken)
    {
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        var productsTask = consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus, message.Site);
        await Task.WhenAll(enteredResultTask, productsTask);

        var (productViewModel, productDetailXml) = await BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds, await productsTask);
        var reportData = await GetWinLossDetail(message, productViewModel.UserSelectedProductIds, productDetailXml);

        return new ConsolidationDetailViewModel(
            message,
            productViewModel,
            BuildBreadcrumbViewModel(message),
            await enteredResultTask,
            reportData);
    }

    public async Task<ExcelFileViewModel> Handle(ConsolidationDetailExcelQuery message, CancellationToken cancellationToken)
    {
        var products = await consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus, message.Site);
        var (validUserSelectedProductIds, productDetailXml) = await FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds,
            products);
        var reportData = await GetWinLossDetail(
            message,
            validUserSelectedProductIds.JoinWithComma(),
            productDetailXml);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return winLossDetailWorkbook.BuildExcelFile(
            message.FromDate,
            message.ToDate,
            message.CustName,
            message.CustLevel,
            false,
            reportData,
            productNames);
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
            null,
            message.Site);

        return winlossDetailService.GetWinLossDetail(param);
    }

    protected override BreadcrumbViewModel BuildBreadcrumbViewModel(BaseQuery message)
    {
        var breadcrumbs = new List<Breadcrumb>
        {
            new Breadcrumb(message.ViewerId, Resources.Consolidation, message.ViewerLevelId, IsConsolidation: true)
        };

        if (JsonHelper.TryParse<Breadcrumb[]>(message.BreadcrumbsJson, out var breadcrumbList))
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
}