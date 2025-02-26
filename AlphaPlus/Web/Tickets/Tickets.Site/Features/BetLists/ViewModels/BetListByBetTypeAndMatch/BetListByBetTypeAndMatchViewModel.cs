using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetTypeAndMatch;

public class BetListByBetTypeAndMatchViewModel : BetListByBetTypeViewModel
{
    public BetListByBetTypeAndMatchViewModel(
        BetListByBetTypeAndMatchQuery message,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper,
        HashSet<int> supportScoreMapBetTypeIds = null)
            : base(message, betListData, builder, ticketHelper, supportScoreMapBetTypeIds)
    {
        MatchId = message.MatchId;
        IsLive = message.IsLive;
        BetTeam = message.BetTeam;

        var firstTicket = betListData.Tickets.FirstOrDefault();
        if (firstTicket != null)
        {
            LeagueId = firstTicket.LeagueId;
            LeagueGroupId = firstTicket.LeagueGroupId;
        }
    }

    public int MatchId { get; }

    public int IsLive { get; }

    public string BetTeam { get; }

    public int LeagueId { get; }

    public int LeagueGroupId { get; }

    protected override string GetTitle()
        => BetTypeConstants.IsOutright(BetType)
        ? CacheRefServiceHelper.GetBetTypeName(BetType)
        : LeagueId == 0
            ? CacheRefServiceHelper.GetBetTypeName(BetType)
            : $"{CacheRefServiceHelper.GetLeagueName(LeagueId, LeagueGroupId)} {CacheRefServiceHelper.GetBetTypeName(BetType)}";

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(MatchId)] = MatchId.ToString(),
            [nameof(BetType)] = BetType.ToString(),
            [nameof(IsLive)] = IsLive.ToString(),
            [nameof(BetTeam)] = BetTeam
        };
}
