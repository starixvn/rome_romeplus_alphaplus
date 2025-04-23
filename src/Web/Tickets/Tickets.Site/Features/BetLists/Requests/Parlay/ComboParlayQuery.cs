using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;

public class ComboParlayQuery : BetListBaseQuery, IRequest<ComboParlayViewModel>
{
    public long TransId { get; set; }

    public decimal Handicap1 { get; set; }

    public decimal Handicap2 { get; set; }

    public string BetCheck { get; set; }

    public string BetTeam { get; set; }
}