using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailGetDataQuery : WinLossDetailNewQuery, IRequest<WinLossDetailGetDataViewModel>
{
    public bool GetWeekData { get; set; }

    public DateTime? WeekFromDate { get; set; }

    public DateTime? WeekToDate { get; set; }
}