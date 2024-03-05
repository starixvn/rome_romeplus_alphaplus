using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class BaseHandler
{
    protected static ProductMultiSelectionViewModel BuildProductMultiSelectionViewModel(
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
        }

        return new ProductMultiSelectionViewModel(products, selectedProductIds.JoinWithComma(), language);
    }

    protected IEnumerable<Breadcrumb> BuildBreadcrumbLevel(BaseQuery message)
    {
        var breadcrumbLevel = new List<Breadcrumb>();

        if (message.BreadcrumbLevel?.Any() == true)
        {
            foreach (var breadcrumb in message.BreadcrumbLevel)
            {
                if (breadcrumb.CustLevelId == message.CustLevelId)
                {
                    break;
                }

                breadcrumbLevel.Add(breadcrumb);
            }
        }

        breadcrumbLevel.Add(new Breadcrumb(message.CustId, message.CustName, message.CustLevelId));

        return breadcrumbLevel;
    }
}
