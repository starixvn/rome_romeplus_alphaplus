namespace AlphaPlus.Reports.Features.Shared.Models;

public class TimeRange(DateTime fromDate, DateTime toDate)
{
    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;
}