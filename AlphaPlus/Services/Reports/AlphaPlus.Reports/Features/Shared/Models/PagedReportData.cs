namespace AlphaPlus.Reports.Features.Shared.Models;

public class PagedReportData<T>(
    IEnumerable<T> dataSource,
    int currentPageIndex,
    int pageSize,
    int totalRecords,
    T totalData) : ReportData<T>(dataSource, totalData) where T : class
{
    public int CurrentPageIndex { get; } = currentPageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRecords { get; } = totalRecords;

    public override bool Any() => TotalRecords > 0;

    public override bool MoreThanOne() => TotalRecords > 1;
}