using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;

public class BetListByBetTypeViewModel(
    BetListByBetTypeQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    HashSet<int> supportScoreMapBetTypeIds = null)
        : BetListBaseViewModel(message, betListData, builder, ticketHelper, true)
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

    public bool AllowToViewSuperPT => ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT => ViewerLevel.IsMasterOrSuperLevel();

    protected override string GetTitle() => CacheRefServiceHelper.GetBetTypeName(BetType);

    public override bool CanViewScoreMap(ITicket ticket)
        => supportScoreMapBetTypeIds?.Contains(ticket.BetTypeId) == true;

    public string FuncName
        => BetTypes.TryGetValue(BetType, out var functionName) ? functionName : string.Empty;
}
