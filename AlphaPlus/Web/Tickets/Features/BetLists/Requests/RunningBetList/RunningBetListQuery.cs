using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RunningBetList;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.RunningBetList;

public class RunningBetListQuery : BetListBaseQuery, IRequest<RunningBetListViewModel>
{
    public RunningBetListQuery() : base()
    {
        MinDate = ReportDateService.Instance.GetMoveBetsDay().GetAwaiter().GetResult().AddDays(1);
        FromDate = SystemTime.Today.AddDays(-7);
        ToDate = SystemTime.Today;
        AutoFollowBetIsDummyAgent = Viewer.AutoFollowBetIsDummyAgent;
        LimitedDateToShowInfo = Viewer.LimitedDateToShowInfo;
    }

    public bool GetAll { get; set; }

    public bool IsFilterVisible { get; set; } = true;

    public int UserSelectedDateRangeOption { get; set; } = DateRangePickerViewModel.SpecificDateOptionValue;

    public DateTime MinDate { get; }

    public bool AutoFollowBetIsDummyAgent { get; }

    public DateTime LimitedDateToShowInfo { get; }
}
