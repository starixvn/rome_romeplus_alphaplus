using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetTypeAndMatch;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;

public class BetListByBetTypeAndMatchQuery : BetListByBetTypeQuery, IRequest<BetListByBetTypeAndMatchViewModel>
{
    public int MatchId { get; set; }

    public int IsLive { get; set; }

    public string BetTeam { get; set; }
}