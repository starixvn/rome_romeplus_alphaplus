using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewQuery : ProductDetailDateRangeQuery, IRequest<WinLossDetailNewViewModel>
{
    public WinLossDetailNewQuery() : base()
    {
        MinDate = SystemTime.Today.AddYears(-3).FirstDayOfYear();
        IsProtectedMode = Viewer.IsProtectedMode;
    }

    public string Username { get; set; }

    public DateTime? WeekFromDate { get; set; }

    public DateTime? WeekToDate { get; set; }

    public string VisibleColumns { get; set; }

    public bool IsProtectedMode { get; }

    public override bool IsValid()
        => IsValidWeekRange() && base.IsValid() && ToDate.Subtract(FromDate).TotalDays <= 92;

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