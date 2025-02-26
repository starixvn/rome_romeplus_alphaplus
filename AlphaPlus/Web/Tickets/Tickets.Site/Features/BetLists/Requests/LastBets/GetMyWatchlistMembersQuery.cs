using AlphaPlus.BetLists.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class GetMyWatchlistMembersQuery : BetListBaseQuery, IRequest<IEnumerable<MyWatchlistMember>>
{
}