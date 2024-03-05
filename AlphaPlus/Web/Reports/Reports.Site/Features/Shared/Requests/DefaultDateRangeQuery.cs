using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class DefaultDateRangeQuery : BaseQuery
{
    public DateTime FromDate { get; set; } = DateTime.MinValue;

    public DateTime ToDate { get; set; } = DateTime.Today;

    public int UserSelectedDateRangeOption { get; set; }

    public bool IsSupportHistoricReport { get; }

    public bool IsHistoricReport { get; set; }

    public override bool IsValid() => base.IsValid() && CheckDateRangeConstraint();

    public bool CheckDateRangeConstraint()
    {
        //// TODO: Side effect here
        AssignFromDateValue();

        return FromDate <= ToDate
            && FromDate >= MinDate
            && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
    }

    private void AssignFromDateValue()
    {
        var today = DateTime.Today;
        if (FromDate == DateTime.MinValue)
        {
            FromDate = IsHistoricReport ? today.FirstDayOfMonth() : today;
        }

        MinDate = IsHistoricReport ? today.FirstDayOf3YearsAgo() : today.FirstDayOf3MonthsAgo();
    }
}
