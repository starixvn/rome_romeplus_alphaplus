using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CombinationMixParlay;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CombinationMixParlay;

public class CombinationMixParlayQuery : BetListByBetTypeQuery, IRequest<CombinationMixParlayViewModel>
{
    public CombinationMixParlayQuery() : base()
        => BetType = BetTypeConstants.CombineMixParlay;
}