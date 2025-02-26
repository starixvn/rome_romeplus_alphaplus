using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.MemberBetList;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.MemberBetList;

public class MemberBetListQuery : BetListBaseQuery, IRequest<MemberBetListViewModel>
{
    public MemberBetListQuery() : base()
    {
        MinDate = ReportDateService.Instance.GetMoveBetsDay().GetAwaiter().GetResult().AddDays(1);
        FromDate = SystemTime.Today;
        ToDate = SystemTime.Today;
        LimitedDateToShowInfo = Viewer.LimitedDateToShowInfo;
    }

    public bool IsFilterVisible { get; set; } = true;

    public int UserSelectedDateRangeOption { get; set; }

    public DateTime MinDate { get; }

    public DateTime LimitedDateToShowInfo { get; }
}