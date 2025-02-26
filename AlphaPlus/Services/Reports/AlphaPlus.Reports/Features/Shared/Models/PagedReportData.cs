namespace AlphaPlus.Reports.Features.Shared.Models;

public class PagedReportData<T>(
    IEnumerable<T> dataSource,
    T total,
    int pageIndex,
    int pageSize,
    int totalRecords) : ReportData<T>(dataSource, total, totalRecords) where T : class
{
    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public new bool HasData { get; } = totalRecords > 0;

    public new bool MoreThanOne { get; } = totalRecords > 1;
}