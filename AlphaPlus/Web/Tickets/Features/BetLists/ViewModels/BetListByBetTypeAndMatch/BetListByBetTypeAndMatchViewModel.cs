using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetTypeAndMatch;

public class BetListByBetTypeAndMatchViewModel(
    BetListByBetTypeAndMatchQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    HashSet<int> supportScoreMapBetTypeIds = null)
        : BetListByBetTypeViewModel(message, betListData, builder, ticketHelper, supportScoreMapBetTypeIds)
{
    public int MatchId { get; } = message.MatchId;

    public int IsLive { get; } = message.IsLive;

    public string BetTeam { get; } = message.BetTeam;

    public int LeagueId { get; } = betListData.Total > 0 ? betListData.Tickets.First().LeagueId : 0;

    protected override string GetTitle()
        => BetTypeConstants.IsOutright(message.BetType)
        ? CacheRefServiceHelper.GetBetTypeName(message.BetType)
        : LeagueId == 0
            ? CacheRefServiceHelper.GetBetTypeName(message.BetType)
            : $"{CacheRefServiceHelper.GetLeagueName(LeagueId)} {CacheRefServiceHelper.GetBetTypeName(message.BetType)}";

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(message.MatchId)] = message.MatchId.ToString(),
            [nameof(message.BetType)] = message.BetType.ToString(),
            [nameof(message.IsLive)] = message.IsLive.ToString(),
            [nameof(message.BetTeam)] = message.BetTeam
        };
}