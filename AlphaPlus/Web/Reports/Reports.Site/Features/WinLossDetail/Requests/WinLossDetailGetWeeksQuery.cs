using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailGetWeeksQuery : ProductDetailDateRangeQuery, IRequest<IEnumerable<WinLossDetailWeekViewModel>>
{
    public WinLossDetailGetWeeksQuery()
        => MinDate = LimitedDateToShowInfo == DateTime.MinValue
        ? SystemTime.Today.AddYears(-3).FirstDayOfYear()
        : LimitedDateToShowInfo;

    public string Username { get; set; }

    public DateTime? WeekFromDate { get; set; }

    public DateTime? WeekToDate { get; set; }

    public byte LevelType { get; set; }

    public override bool IsValid()
        => CustLevel.IsNotMemberLevel()
        && IsValidWeekRange()
        && base.IsValid()
        && ToDate.Subtract(FromDate).TotalDays <= 92;

    private bool IsValidWeekRange()
    {
        if (!WeekFromDate.HasValue && !WeekToDate.HasValue)
        {
            return true;
        }

        return WeekFromDate.HasValue
            && WeekToDate.HasValue
            && WeekFromDate.Value >= FromDate
            && WeekToDate.Value <= ToDate;
    }
}
