using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;

public class ComboParlayDetailQuery : BetListBaseQuery, IRequest<ComboParlayDetailViewModel>
{
    public long RefNo { get; set; }

    public DateTime WinLostDate { get; set; }

    public string BetTeam { get; set; }
}
