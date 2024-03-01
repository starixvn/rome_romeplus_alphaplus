using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class BaseHandler
{
    protected internal static ProductMultiSelectionViewModel BuildProductMultiSelectionViewModel(
        IEnumerable<Product> offeredProducts,
        string userSelectedProductIds,
        string language)
    {
        var products = new List<SelectListItem>();
        var selectedProductIds = userSelectedProductIds.ToListInt();

        if (offeredProducts.Any())
        {
            var groupOfferedProducts = offeredProducts.GroupBy(product => product.TypeName);
            foreach (var productList in groupOfferedProducts)
            {
                products.AddRange(productList.Select(product => new SelectListItem(
                    product.DisplayName,
                    product.Id.ToString(),
                    selectedProductIds.Contains(product.Id)
                )
                {
                    Group = string.IsNullOrWhiteSpace(productList.Key)
                        ? null
                        : new SelectListGroup { Name = productList.Key }
                }));
            }
        }

        return new ProductMultiSelectionViewModel(products, string.Join(',', selectedProductIds), language);
    }

    protected internal IEnumerable<Breadcrumb> BuildBreadcrumbLevel(BaseQuery message)
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
