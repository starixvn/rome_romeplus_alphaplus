using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class BaseHandler(IProductService productService)
{
    protected readonly IProductService productService = productService;

    protected ProductMultiSelectionViewModel BuildProductViewModel(
        BaseQuery message,
        string userSelectedProductIds,
        IEnumerable<int> offeredProducts = null)
    {
        var allProducts = productService.GetOfferedProducts(message.Site, message.CurrencyId, message.MinDate);
        if (offeredProducts != null)
        {
            allProducts = allProducts
                .Where(product => offeredProducts.Contains(product.Id))
                .Select(product =>
                {
                    product.TypeName = string.Empty;
                    return product;
                })
                .ToList();
        }

        return allProducts.Any()
            ? BuildProductViewModel(allProducts, userSelectedProductIds, message.Language)
            : new ProductMultiSelectionViewModel([], string.Empty, message.Language);
    }

    private static ProductMultiSelectionViewModel BuildProductViewModel(
        IEnumerable<Product> offeredProducts,
        string userSelectedProductIds,
        string language)
    {
        var products = new List<SelectListItem>();
        var selectedProductIds = new List<int>();

        var isSelectedAll = string.IsNullOrWhiteSpace(userSelectedProductIds);
        var userSelectedProductIdsList = isSelectedAll ? [] : userSelectedProductIds.ToListInt();

        var groupOfferedProducts = offeredProducts.GroupBy(product => product.TypeName);
        foreach (var productList in groupOfferedProducts)
        {
            foreach (var product in productList)
            {
                var isSelected = isSelectedAll || userSelectedProductIdsList.Contains(product.Id);
                products.Add(new SelectListItem(product.DisplayName, product.Id.ToString(), isSelected)
                {
                    Group = string.IsNullOrWhiteSpace(productList.Key)
                        ? null
                        : new SelectListGroup { Name = productList.Key }
                });

                if (isSelected)
                {
                    selectedProductIds.Add(product.Id);
                }
            }
        }

        return new ProductMultiSelectionViewModel(products, selectedProductIds.JoinWithComma(), language);
    }

    protected IEnumerable<int> FilterProductIds(BaseQuery message, string userSelectedProductIds, IEnumerable<int> offeredProducts = null)
    {
        var allProducts = productService.GetOfferedProducts(message.Site, message.CurrencyId, message.MinDate);
        if (offeredProducts != null)
        {
            allProducts = allProducts.Where(product => offeredProducts.Contains(product.Id));
        }

        var userSelectedProductIdsList = userSelectedProductIds.ToListInt();
        if (userSelectedProductIdsList.Any())
        {
            allProducts = allProducts.Where(product => userSelectedProductIdsList.Contains(product.Id));
        }

        return allProducts.Select(product => product.Id).ToList();
    }

    protected static BreadcrumbViewModel BuildBreadcrumbViewModel(BaseQuery message)
    {
        var breadcrumbs = new List<Breadcrumb>();

        if (JsonHelper.TryParse<Breadcrumb[]>(message.BreadcrumbsJson, out var breadcrumbList))
        {
            foreach (var item in breadcrumbList)
            {
                if (item.CustLevelId == message.CustLevelId)
                {
                    break;
                }

                breadcrumbs.Add(item);
            }
        }

        breadcrumbs.Add(new Breadcrumb(message.CustId, message.CustName, message.CustLevelId));

        return new BreadcrumbViewModel(breadcrumbs, JsonHelper.ToJson(breadcrumbs));
    }
}
