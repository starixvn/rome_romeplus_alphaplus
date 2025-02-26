using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class LastBetsQuery : BetListBaseQuery, IRequest<LastBetsViewModel>
{
    public int BetType { get; set; } = BetTypeConstants.AllBetTypes;

    public int PageSize { get; set; } = 10;
}