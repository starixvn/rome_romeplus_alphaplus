using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLossByMatch;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLossByMatch;

public class WinLossByMatchQuery : BetListBaseQuery, IRequest<WinLossByMatchViewModel>
{
    public int BetType { get; set; }

    public int LeagueId { get; set; }

    public int MatchId { get; set; }

    public string Race { get; set; }

    public int LiveIndicator { get; set; }

    public byte CustLevelId { get; set; }
}