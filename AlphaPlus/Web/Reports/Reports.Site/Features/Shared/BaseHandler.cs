using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class BaseHandler
{
    protected internal static IEnumerable<SelectListItem> BuildProductSelectListItems(
        IEnumerable<int> userSelectedProductIds,
        IEnumerable<Product> products)
    {
        List<SelectListItem> productList = new();

        if (products?.Any() == true)
        {
            var groupProducts = products.GroupBy(product => product.TypeName);
            foreach (var group in groupProducts)
            {
                productList.AddRange(group.Select(product => new SelectListItem(
                    product.DisplayName,
                    product.Id.ToString(),
                    userSelectedProductIds.Contains(product.Id)
                )
                {
                    Group = string.IsNullOrWhiteSpace(group.Key)
                        ? null
                        : new SelectListGroup { Name = group.Key }
                }));
            }
        }

        return productList;
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
