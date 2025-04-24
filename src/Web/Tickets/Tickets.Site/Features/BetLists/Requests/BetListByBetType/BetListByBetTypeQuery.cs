using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;

public class BetListByBetTypeQuery : BetListBaseQuery, IRequest<BetListByBetTypeViewModel>
{
    public int BetType { get; set; }
}