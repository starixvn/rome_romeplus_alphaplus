using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;

public class WinLossNewQuery : WinLossWeekSelectionQuery, IRequest<WinLossNewViewModel>
{
    public override DateTime MinDate
        => SystemTime.Today.AddYears(NegativeThree)
        .FirstDayOfYear()
        .GetGreaterDate(LimitedDateToShowInfo);

    public DateTime? WeekFromDate { get; set; }

    public DateTime? WeekToDate { get; set; }

    public string VisibleColumns { get; set; }

    public override bool IsValid() => base.IsValid() && IsValidWeekRange();

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
