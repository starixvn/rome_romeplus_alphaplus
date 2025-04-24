namespace AlphaPlus.BetLists.Shared.Models;

public class PagedReportData<T>(
    IEnumerable<T> dataSource,
    int pageIndex,
    int pageSize,
    int totalRecords) where T : class
{
    public IEnumerable<T> DataSource { get; } = dataSource;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRecords { get; } = totalRecords;

    public bool Any() => TotalRecords > 0;

    public bool MoreThanOne() => TotalRecords > 1;
}