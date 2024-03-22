using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingHandler(
    IProductService productService,
    IOutstandingService outstandingService,
    IBrowserHelper browserHelper) : BaseHandler(productService), IRequestHandler<OutstandingQuery, OutstandingViewModel>
{
    // Always hide Saba Promotion in Product List
    private readonly int SabaPromotionProductId = 37;

    public async Task<OutstandingViewModel> Handle(OutstandingQuery message, CancellationToken cancellationToken)
    {
        var products = productService
            .GetOfferedProducts(message.Site, message.CurrencyId, message.MinDate)
            .Where(p => p.Id != SabaPromotionProductId);
        var productViewModel = BuildProductViewModel(products, message.UserSelectedProductIds, message.Language);
        var param = new OutstandingParam(
            message.CustLevel,
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.AutoFollowBetIsDummyAgent,
            message.Site);
        var reportData = await outstandingService.GetOutstanding(param);
        var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

        return new OutstandingViewModel(
            message,
            reportData,
            productViewModel,
            BuildBreadcrumbViewModel(message),
            isOnlyMobile);
    }
}
