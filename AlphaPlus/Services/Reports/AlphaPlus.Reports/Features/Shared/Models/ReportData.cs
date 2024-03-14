namespace AlphaPlus.Reports.Features.Shared.Models;

public class ReportData<T> where T : class
{
    public ReportData(IEnumerable<T> dataSource, T total)
    {
        DataSource = dataSource ?? [];
        Total = total;

        var dataSourceLength = DataSource.Count();
        HasData = dataSourceLength > 0;
        MoreThanOne = dataSourceLength > 1;
    }

    public IEnumerable<T> DataSource { get; }

    public T Total { get; }

    public virtual bool HasData { get; }

    public virtual bool MoreThanOne { get; }
}