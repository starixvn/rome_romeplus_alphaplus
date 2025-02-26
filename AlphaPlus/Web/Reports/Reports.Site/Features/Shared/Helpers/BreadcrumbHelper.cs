using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;

namespace AlphaPlus.Reports.Site.Features.Shared.Helpers;

public static class BreadcrumbHelper
{
    public static IEnumerable<Breadcrumb> BuildBreadcrumbs(BaseQuery message)
    {
        var breadcrumbs = new List<Breadcrumb>();

        if (JsonHelper.TryParse<Breadcrumb[]>(message.BreadcrumbsJson, out var breadcrumbList) && breadcrumbList != null)
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

        return breadcrumbs;
    }
}
