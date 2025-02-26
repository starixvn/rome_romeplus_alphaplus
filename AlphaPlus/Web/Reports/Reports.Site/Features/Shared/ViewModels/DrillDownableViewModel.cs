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

    public byte DownlineLevelId { get; } = (byte)(message.CustLevelId - 1);

    public PageHeaderViewModel PageHeader { get; } = pageHeader;

    public ReportData<T> ReportData { get; } = reportData;

    public string SortPropertyName { get; } = sortPropertyName;

    public byte DescendingSort { get; } = descendingSort;

    protected static BreadcrumbViewModel BuildBreadcrumbViewModel(BaseQuery message)
    {
        var breadcrumbs = BreadcrumbHelper.BuildBreadcrumbs(message);

        return new BreadcrumbViewModel(breadcrumbs, JsonHelper.ToJson(breadcrumbs));
    }
}
