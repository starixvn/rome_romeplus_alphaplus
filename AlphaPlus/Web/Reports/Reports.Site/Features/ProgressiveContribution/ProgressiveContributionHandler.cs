using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Legends;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Legends;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IProgressiveContributionService progressiveContributionService,
    IProgressiveContributionWookbook wookbook,
    ILengendService lengendService) :
        BaseHandler(productService, options),
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>,
        IRequestHandler<ProgressiveContributionExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ProgressiveContributionLegendQuery, LegendViewModel>
{
    public async Task<ProgressiveContributionViewModel> Handle(ProgressiveContributionQuery message, CancellationToken cancellationToken)
    {
        var products = GetProducts(message).Where(p => message.EnableJackpotProductIds.Contains(p.Id));
        var productViewModel = BuildProductViewModel(message, message.UserSelectedProductIds, products);
        var (progressiveContributions, total) = await GetProgressiveContributions(message, productViewModel.UserSelectedProductIds);

        return new ProgressiveContributionViewModel(
            message,
            progressiveContributions,
            total,
            productViewModel);
    }

    public async Task<ExcelFileViewModel> Handle(ProgressiveContributionExcelQuery message, CancellationToken cancellationToken)
    {
        var validUserSelectedProductIds = FilterProductIds(message, message.UserSelectedProductIds, message.EnableJackpotProductIds);
        var (progressiveContributions, total) = await GetProgressiveContributions(message, validUserSelectedProductIds.JoinWithComma());
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return wookbook.BuildExcelFile(message, progressiveContributions, total, productNames);
    }

    private Task<(IEnumerable<ProgressiveContributionModel>, ProgressiveContributionEntity)> GetProgressiveContributions(
        ProgressiveContributionQuery message,
        string validUserSelectedProductIds)
    {
        var param = new ProgressiveContributionParam(
            message.ViewerId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            message.ViewerLevel,
            message.Site);
        return progressiveContributionService.GetProgressiveContributions(param);
    }

    public async Task<LegendViewModel> Handle(ProgressiveContributionLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>
        {
            Resources.ContributionAmountRoundNote
        };

        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate);
        var productNames = await lengendService.GetOfferedProductsNeedDelayTimeToSummarizeData(productParam, message.EnableJackpotProductIds);
        if (!string.IsNullOrWhiteSpace(productNames))
        {
            legends.Add(string.Format(Resources.DelayIssueLegend, productNames));
        }

        return new LegendViewModel(legends);
    }
}
