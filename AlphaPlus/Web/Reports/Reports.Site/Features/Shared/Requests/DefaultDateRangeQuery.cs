using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class DefaultDateRangeQuery : BaseQuery
{
    public DateTime FromDate { get; set; } = DateTime.MinValue;

    public DateTime ToDate { get; set; } = DateTime.Today;

    public int UserSelectedDateRangeOption { get; set; }

    public bool IsSupportHistoricReport { get; protected set; }

    public bool IsHistoricReport { get; set; }

    public override bool IsValid() => base.IsValid() && CheckDateRangeConstraint();

    protected bool CheckDateRangeConstraint()
    {
        //// TODO: Side effect here
        AssignFromDateValue();

        return MinDate <= FromDate
            && FromDate <= ToDate
            && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
    }

    private void AssignFromDateValue()
    {
        var today = DateTime.Today;
        if (FromDate == DateTime.MinValue)
        {
            FromDate = IsHistoricReport ? today.FirstDayOfMonth() : today;
        }

        if (MinDate == DateTime.MinValue)
        {
            MinDate = IsHistoricReport ? today.FirstDayOf3YearsAgo() : today.FirstDayOf3MonthsAgo();
        }
    }
}
