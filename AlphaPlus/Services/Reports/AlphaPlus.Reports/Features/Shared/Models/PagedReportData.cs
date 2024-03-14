namespace AlphaPlus.Reports.Features.Shared.Models;

public class PagedReportData<T>(
    IEnumerable<T> dataSource,
    T total,
    int pageIndex,
    int pageSize,
    int totalRecords) : ReportData<T>(dataSource, total) where T : class
{
    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRecords { get; } = totalRecords;

    public new bool HasData { get; } = totalRecords > 0;

    public new bool MoreThanOne => TotalRecords > 1;
}