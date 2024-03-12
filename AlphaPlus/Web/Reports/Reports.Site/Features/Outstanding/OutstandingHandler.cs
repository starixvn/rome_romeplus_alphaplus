using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingHandler(
    IOutstandingService outstandingService,
    IProductService productService,
    IBrowserHelper browserHelper) : BaseHandler, IRequestHandler<OutstandingQuery, OutstandingViewModel>
{
    // Always hide Saba Promotion in Product List
    private readonly int SabaPromotionProductId = 37;

    public async Task<OutstandingViewModel> Handle(OutstandingQuery message, CancellationToken cancellationToken)
    {
        var products = productService
            .GetOfferedProducts(message.Site, message.CurrencyId, DateTime.Today)
            .Where(p => p.Id != SabaPromotionProductId);
        var productViewModel = BuildProductMultiSelectionViewModel(products, message.UserSelectedProductIds, message.Language);
        var param = new OutstandingParam(
            message.ViewerLevel,
            message.CustLevel,
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.AutoFollowBetIsDummyAgent,
            message.Site);
        var outstandings = await outstandingService.GetOutstanding(param);
        var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

        return new OutstandingViewModel(
            message,
            outstandings,
            productViewModel,
            BuildBreadcrumbs(message),
            isOnlyMobile);
    }
}
