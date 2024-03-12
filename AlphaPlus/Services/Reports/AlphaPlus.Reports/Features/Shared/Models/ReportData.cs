namespace AlphaPlus.Reports.Features.Shared.Models;

public class ReportData<T>(IEnumerable<T> dataSource, T totalData) where T : class
{
    public IEnumerable<T> DataSource { get; } = dataSource ?? [];

    public T TotalData { get; } = totalData;

    public virtual bool Any() => DataSource.Any();

    public virtual bool MoreThanOne() => DataSource.Count() > 1;
}