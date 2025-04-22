using AlphaPlus.Reports.Site.Features.Shared.Requests;

namespace AlphaPlus.Reports.Site.Features.Shared.Helpers;

public static class BreadcrumbHelper
{
    public static IEnumerable<Reports.Features.Shared.Models.Breadcrumb> BuildBreadcrumbs(BaseQuery message)
    {
        var breadcrumbs = new List<Reports.Features.Shared.Models.Breadcrumb>();

        if (JsonHelper.TryParse<Reports.Features.Shared.Models.Breadcrumb[]>(message.BreadcrumbsJson, out var breadcrumbList) && breadcrumbList != null)
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

        breadcrumbs.Add(new Reports.Features.Shared.Models.Breadcrumb(message.CustId, message.CustName, message.CustLevelId));

        return breadcrumbs;
    }
}
