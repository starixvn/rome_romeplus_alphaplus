using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels;

public abstract class DrillDownableViewModel<T>(
    BaseQuery message,
    PageHeaderViewModel pageHeader,
    ReportData<T> reportData,
    string sortPropertyName,
    byte descendingSort) where T : class
{
    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public UserLevel CustLevel { get; } = message.CustLevel;

    public PageHeaderViewModel PageHeader { get; } = pageHeader;

    public ReportData<T> ReportData { get; } = reportData;

    public string SortPropertyName { get; } = sortPropertyName;

    public byte DescendingSort { get; } = descendingSort;

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
