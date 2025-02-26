using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CashOutHistory;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CashOutHistory;

public class CashOutHistoryQuery : BetListBaseQuery, IRequest<CashOutHistoryViewModel>
{
    public long TransId { get; set; }

    public DateTime WinLostDate { get; set; }
}