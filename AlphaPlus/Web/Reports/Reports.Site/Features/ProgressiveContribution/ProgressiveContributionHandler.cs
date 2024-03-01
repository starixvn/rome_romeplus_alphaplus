using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionHandler(
    IProductService productService,
    IProgressiveContributionService progressiveContributionService) :
        BaseHandler,
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>
{
    public async Task<ProgressiveContributionViewModel> Handle(ProgressiveContributionQuery message, CancellationToken cancellationToken)
    {
        var products = productService.GetProgressiveJackpotProduct(message.Site, message.CurrencyId, message.EnableJackpotProductIds);
        var productViewModel = BuildProductMultiSelectionViewModel(products, message.UserSelectedProductIds, message.Language);
        var param = new ProgressiveContributionParam(
            message.CustId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            productViewModel.UserSelectedProductIds,
            message.ViewerLevel,
            message.Site);
        var (progressiveContributions, total) = await progressiveContributionService.GetProgressiveContributions(param);

        return new ProgressiveContributionViewModel(
            message,
            progressiveContributions,
            total,
            productViewModel);
    }
}