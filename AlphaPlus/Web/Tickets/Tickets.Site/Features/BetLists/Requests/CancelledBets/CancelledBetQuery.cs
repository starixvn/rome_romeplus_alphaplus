using AlphaPlus.BetLists.CancelledBets;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CancelledBets;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CancelledBets;

public class CancelledBetQuery : BetListBaseQuery, IRequest<CancelledBetViewModel>
{
    public CancelledBetQuery() : base()
    {
        FromDate = FromDate == DateTime.MinValue ? SystemTime.Today : FromDate;
        ToDate = ToDate == DateTime.MinValue ? SystemTime.Today : ToDate;
    }

    public bool IsFilterVisible { get; set; } = true;

    public string StatusIds { get; set; } = TicketStatusConstants.All.JoinWithComma();

    public int UserSelectedDateRangeOption { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 50;

    public DateTime MinDate { get; } = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();
}