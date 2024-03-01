using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class DefaultDateRangeQuery : BaseQuery
{
    public DefaultDateRangeQuery(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool isSupportHistoricReport = true,
        bool isHistoricReport = false,
        DateTime? minDate = null) : base()
    {
        var today = DateTime.Today;
        FromDate = fromDate ?? today.Yesterday();
        ToDate = toDate ?? today;
        IsSupportHistoricReport = isSupportHistoricReport;
        IsHistoricReport = isSupportHistoricReport && isHistoricReport;
        MinDate = minDate ?? (isHistoricReport ? today.FirstDayOf3YearsAgo() : today.FirstDayOf3MonthsAgo());
    }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int UserSelectedDateRangeOption { get; set; }

    public bool IsSupportHistoricReport { get; }

    public bool IsHistoricReport { get; set; }

    public override bool IsValid() => base.IsValid() && CheckDateRangeConstraint();

    public bool CheckDateRangeConstraint()
        => FromDate <= ToDate
        && FromDate >= MinDate
        && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
}
