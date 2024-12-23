using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;

public class BetListByBetTypeViewModel(
    BetListByBetTypeQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    HashSet<int> supportScoreMapBetTypeIds = null,
    bool supportRiskAmount = true) : BetListBaseViewModel(message, betListData, builder, ticketHelper, true, supportRiskAmount)
{
    private readonly IDictionary<int, string> BetTypes = new Dictionary<int, string>
    {
        [13] = "CleanSheet",
        [24] = "DoubleChance",
        [26] = "BothNoneNeitherTeamToScore",
        [27] = "ToWinToNil",
        [28] = "ThreeWayHandicap"
    };

    public int BetType { get; } = message.BetType;

    public bool AllowToViewSuperPT { get; } = message.ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT { get; } = message.ViewerLevel.IsMasterOrSuperLevel();

    protected override string GetTitle() => CacheRefServiceHelper.GetBetTypeName(BetType).ToUpperInvariant();

    public override bool CanViewScoreMap(ITicket ticket)
        => supportScoreMapBetTypeIds?.Contains(ticket.BetTypeId) == true;

    public string FuncName
        => BetTypes.TryGetValue(BetType, out var functionName) ? functionName : string.Empty;
}
