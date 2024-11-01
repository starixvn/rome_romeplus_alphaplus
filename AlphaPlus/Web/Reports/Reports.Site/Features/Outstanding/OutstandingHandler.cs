using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IOutstandingService outstandingService,
    IBrowserHelper browserHelper) :
        BaseHandler(productService, options),
        IRequestHandler<OutstandingQuery, OutstandingViewModel>
{
    // Always hide Saba Promotion in Product List
    private readonly int SabaPromotionProductId = 37;

    public async Task<OutstandingViewModel> Handle(OutstandingQuery message, CancellationToken cancellationToken)
    {
        var products = await GetProductsAsync(message);
        products = products.Where(p => p.Id != SabaPromotionProductId);

        var productViewModel = BuildProductViewModel(message, message.UserSelectedProductIds, products);
        var outstandingParam = new OutstandingParam(
            message.CustLevel.Value,
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.AutoFollowBetIsDummyAgent,
            message.Site);
        var reportData = await outstandingService.GetOutstanding(outstandingParam);
        var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

        return new OutstandingViewModel(
            message,
            reportData,
            productViewModel,
            BuildBreadcrumbViewModel(message),
            isOnlyMobile);
    }
}
