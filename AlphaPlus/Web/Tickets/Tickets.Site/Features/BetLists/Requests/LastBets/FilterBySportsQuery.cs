using AlphaPlus.BetLists.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class FilterBySportsQuery : BetListBaseQuery, IRequest<IEnumerable<Sport>>
{
    public byte GroupSport { get; set; }
}
