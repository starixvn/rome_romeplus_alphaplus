using AlphaPlus.Reports.Features.Legends;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Legends;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionHandler(
    IProductService productService,
    IProgressiveContributionService progressiveContributionService,
    IProgressiveContributionWookbook wookbook,
    ILengendService lengendService) :
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>,
        IRequestHandler<ProgressiveContributionExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ProgressiveContributionLegendQuery, LegendViewModel>
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

    public async Task<ExcelFileViewModel> Handle(ProgressiveContributionExcelQuery message, CancellationToken cancellationToken)
    {
        var products = productService.GetProgressiveJackpotProduct(message.Site, message.CurrencyId, message.EnableJackpotProductIds);
        if (!string.IsNullOrWhiteSpace(message.UserSelectedProductIds))
        {
            var userSelectedProductIds = message.UserSelectedProductIds.ToListInt();
            products = products.Where(p => userSelectedProductIds.Contains(p.Id));
        }

        var param = new ProgressiveContributionParam(
            message.CustId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            products.Select(p => p.Id).JoinWithComma(),
            message.ViewerLevel,
            message.Site);
        var (progressiveContributions, total) = await progressiveContributionService.GetProgressiveContributions(param);

        return wookbook.BuildExcelFile(message, progressiveContributions, total);
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
