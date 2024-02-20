using AlphaPlus.BetLists.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;

public class BetListByBetTypeViewModel : BetListBaseViewModel
{
    private readonly IDictionary<int, string> BetTypes = new Dictionary<int, string>
    {
        [13] = "CleanSheet",
        [24] = "DoubleChance",
        [26] = "BothNoneNeitherTeamToScore",
        [27] = "ToWinToNil",
        [28] = "ThreeWayHandicap"
    };

    public BetListByBetTypeViewModel(
        BetListByBetTypeQuery query,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper,
        IEnumerable<int> supportScoreMapBetTypeIds = null)
        : base(query, betListData, builder, ticketHelper, true)
    {
        SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;
        Title = CacheRefServiceHelper.GetBetTypeName(query.BetType).ToUpperInvariant();
        BetType = query.BetType;
    }

    public int BetType { get; }

    public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

    public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

    public override bool CanViewScoreMap(ITicket ticket)
        => SupportScoreMapBetTypeIds != null
        && SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId);

    public string FuncName
        => BetTypes.Keys.Any(key => key == BetType) ? BetTypes[BetType] : string.Empty;
}
