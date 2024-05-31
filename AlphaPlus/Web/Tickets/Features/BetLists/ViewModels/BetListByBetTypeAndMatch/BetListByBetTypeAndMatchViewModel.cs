using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetTypeAndMatch;

public class BetListByBetTypeAndMatchViewModel : BetListByBetTypeViewModel
{
    public BetListByBetTypeAndMatchViewModel(
        BetListByBetTypeAndMatchQuery query,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper,
        IEnumerable<int> supportScoreMapBetTypeIds = null)
        : base(query, betListData, builder, ticketHelper, supportScoreMapBetTypeIds)
    {
        MatchId = query.MatchId;
        IsLive = query.IsLive;
        BetTeam = query.BetTeam;
        LeagueId = betListData.Tickets.Any() ? betListData.Tickets.FirstOrDefault().LeagueId : 0;
        Title = GetTitle(query.BetType).ToUpperInvariant();
    }

    public int MatchId { get; }

    public int IsLive { get; }

    public string BetTeam { get; }

    public int LeagueId { get; }

    public string GetTitle(int betType)
        => BetTypeConstants.IsOutright(betType)
        ? CacheRefServiceHelper.GetBetTypeName(betType)
        : LeagueId == 0
            ? CacheRefServiceHelper.GetBetTypeName(betType)
            : $"{CacheRefServiceHelper.GetLeagueName(LeagueId)} {CacheRefServiceHelper.GetBetTypeName(betType)}";
}