using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class GetLastBetsDataNewQuery : BetListBaseQuery, IRequest<LastBetsDataNewViewModel>
{
    public int FilterId { get; set; }

    public long TransId { get; set; }

    public string MemberList { get; set; }

    public string BetTypeList { get; set; }

    public string SportTypeList { get; set; }

    public string LeagueList { get; set; }

    public byte EvenStatus { get; set; }

    public decimal MinStake { get; set; }

    public int LimitRowCount { get; set; }

    public long LatestTransId { get; set; }

    public byte FilterByMembers { get; set; }

    public byte ViewerLevelId { get; set; }
}