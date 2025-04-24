using AlphaPlus.Core.Utils;
using AlphaPlus.Core.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class DefaultDateRangeQuery : BaseQuery
{
    public DateTime FromDate { get; set; } = DateTime.MinValue;

    public DateTime ToDate { get; set; } = SystemTime.Today;

    public int UserSelectedDateRangeOption { get; set; }

    public bool IsHistoricReport { get; set; }

    public override bool IsValid() => base.IsValid() && CheckDateRangeConstraint();

    public bool CheckDateRangeConstraint()
    {
        AssignFromDateValue();

        return MinDate <= FromDate
            && FromDate <= ToDate
            && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
    }

    private void AssignFromDateValue()
    {
        var today = SystemTime.Today;
        if (FromDate == DateTime.MinValue)
        {
            FromDate = IsHistoricReport ? today.FirstDayOfMonth() : today;
        }

        if (MinDate == DateTime.MinValue)
        {
            MinDate = IsHistoricReport ? today.AddYears(-3).FirstDayOfYear() : today.AddMonths(-3).FirstDayOfMonth();
        }
    }
}
