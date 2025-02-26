using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
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


    private readonly IOutstandingService outstandingService = outstandingService;
    private readonly IBrowserHelper browserHelper = browserHelper;

    public async Task<OutstandingViewModel> Handle(OutstandingQuery message, CancellationToken cancellationToken)
    {
        var productViewModel = await BuildProductViewModel(message, message.UserSelectedProductIds);
        var outstandingParam = new OutstandingParam(
            message.CustLevelId,
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.AutoFollowBetIsDummyAgent,
            message.Site);
        var reportData = await outstandingService.GetOutstanding(outstandingParam);
        var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

        return new OutstandingViewModel(message, reportData, productViewModel, isOnlyMobile);
    }

    protected override async Task<IEnumerable<Product>> GetProducts(BaseQuery message)
    {
        var products = await base.GetProducts(message);

        // Always hide Saba Promotion in Product List
        const int SabaPromotionProductId = 37;

        return products.Where(p => p.Id != SabaPromotionProductId);
    }
}
