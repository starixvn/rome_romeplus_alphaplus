using AlphaPlus.Reports.Features.Legends;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Legends;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionHandler(
    IProductService productService,
    IProgressiveContributionService progressiveContributionService,
    IProgressiveContributionWookbook wookbook,
    ILengendService lengendService) :
        BaseHandler(productService),
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>,
        IRequestHandler<ProgressiveContributionExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ProgressiveContributionLegendQuery, LegendViewModel>
{
    public async Task<ProgressiveContributionViewModel> Handle(ProgressiveContributionQuery message, CancellationToken cancellationToken)
    {
        var productViewModel = BuildProductViewModel(message, message.UserSelectedProductIds, message.EnableJackpotProductIds);
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
            message.CustId,
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

        var productNames = await lengendService.GetOfferedProductsNeedDelayTimeToSummarizeData(
            message.Site, message.CurrencyId, message.MinDate, message.EnableJackpotProductIds);
        if (!string.IsNullOrWhiteSpace(productNames))
        {
            legends.Add(string.Format(Resources.DelayIssueLegend, productNames));
        }

        return new LegendViewModel(legends);
    }
}
