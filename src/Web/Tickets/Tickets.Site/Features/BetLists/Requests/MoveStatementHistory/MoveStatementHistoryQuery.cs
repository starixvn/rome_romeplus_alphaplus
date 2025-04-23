using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.MoveStatementHistory;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.MoveStatementHistory;

public class MoveStatementHistoryQuery : BetListBaseQuery, IRequest<MoveStatementHistoryViewModel>
{
    public long TransId { get; set; }
}