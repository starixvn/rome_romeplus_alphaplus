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
        var validUserSelectedProductIds = productService.FilterProductIds(
            message.Site,
            message.CurrencyId,
            message.UserSelectedProductIds.ToListInt(),
            DateTime.Now).Where(id => id != SabaPromotionProductId);

        var param = new OutstandingParam(
            message.ViewerLevel,
            message.CustLevel,
            message.CustId,
            string.Join(',', validUserSelectedProductIds),
            message.AutoFollowBetIsDummyAgent,
            message.Site);

        var outstandings = await outstandingService.GetOutstanding(param);
        var products = productService.GetAllOfferedProducts(message.Site, message.CurrencyId, DateTime.Now).Where(p => p.Id != SabaPromotionProductId);
        var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

        return new OutstandingViewModel(
            message,
            outstandings,
            validUserSelectedProductIds,
            BuildProductSelectListItems(validUserSelectedProductIds, products),
            BuildBreadcrumbLevel(message),
            isOnlyMobile);
    }
}
