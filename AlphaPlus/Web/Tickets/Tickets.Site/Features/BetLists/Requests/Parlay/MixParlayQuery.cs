using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;

public class MixParlayQuery : BetListBaseQuery, IRequest<MixParlayViewModel>
{
    public long TransId { get; set; }
}
