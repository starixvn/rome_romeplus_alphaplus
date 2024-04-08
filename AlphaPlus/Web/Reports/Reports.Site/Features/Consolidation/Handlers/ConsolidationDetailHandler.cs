using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Reports.Site.Features.WinLossDetail;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IWinLossDetailService winlossDetailService) :
        BaseHandler(productService, options),
        IRequestHandler<ConsolidationDetailQuery, ConsolidationDetailViewModel>
{
    public async Task<ConsolidationDetailViewModel> Handle(ConsolidationDetailQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);
        var reportDataTask = GetWinLossDetail(message, productViewModel.UserSelectedProductIds, productDetailXml);
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        await Task.WhenAll(reportDataTask, enteredResultTask);

        return new ConsolidationDetailViewModel(
            message,
            productViewModel,
            BuildBreadcrumbViewModel(message),
            await enteredResultTask,
            await reportDataTask);
    }

    private Task<ReportData<WinLossDetailModel>> GetWinLossDetail(
        ConsolidationDetailQuery message,
        string validUserSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossDetailParam(
            message.CustId,
            message.CustLevel,
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
            new Breadcrumb(message.ViewerId, Resources.Consolidation, message.ViewerLevelId)
        };

        if (JsonHelper.TryParse<Breadcrumb[]>(message.BreadcrumbsJson, out var breadcrumbList))
        {
            foreach (var item in breadcrumbList)
            {
                if (item.CustLevelId == message.CustLevelId)
                {
                    break;
                }

                breadcrumbs.Add(item);
            }
        }

        breadcrumbs.Add(new Breadcrumb(message.CustId, message.CustName, message.CustLevelId));

        return new BreadcrumbViewModel(breadcrumbs, JsonHelper.ToJson(breadcrumbs));
    }
}