using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        var productViewModel = BuildProgressiveJackpotProductMultiSelectionViewModel(products, message.UserSelectedProductIds, message.Language);
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

    private static ProductMultiSelectionViewModel BuildProgressiveJackpotProductMultiSelectionViewModel(
        IEnumerable<Product> offeredProducts,
        string userSelectedProductIds,
        string language)
    {
        var products = new List<SelectListItem>();
        var selectedProductIds = new List<int>();

        var isSelectedAll = string.IsNullOrWhiteSpace(userSelectedProductIds);
        var userSelectedProductIdsList = isSelectedAll ? [] : userSelectedProductIds.ToListInt();

        if (offeredProducts.Any())
        {
            foreach (var product in offeredProducts)
            {
                var isSelected = isSelectedAll || userSelectedProductIdsList.Contains(product.Id);
                products.Add(new SelectListItem(product.DisplayName, product.Id.ToString(), isSelected));

                if (isSelected)
                {
                    selectedProductIds.Add(product.Id);
                }
            }
        }

        return new ProductMultiSelectionViewModel(products, selectedProductIds.JoinWithComma(), language);
    }
}