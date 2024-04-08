namespace AlphaPlus.Reports.Features.Shared.Models;

public class ReportData<T> where T : class
{
    public ReportData(IEnumerable<T> dataSource, T total, int? totalRecords = null)
    {
        DataSource = dataSource ?? [];
        Total = total;
        TotalRecords = totalRecords ?? DataSource.Count();
        HasData = TotalRecords > 0;
        MoreThanOne = TotalRecords > 1;
    }

    public IEnumerable<T> DataSource { get; }

    public T Total { get; }

    public int TotalRecords { get; }

    public virtual bool HasData { get; }

    public virtual bool MoreThanOne { get; }
}