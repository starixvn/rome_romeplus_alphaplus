using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Blocks;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;

public class WinLossViewModel(
    BetListBaseQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    bool showContributionAmountRoundNote,
    bool isGroupHeadline = false,
    bool showMemberName = false,
    string[] excludingTicketStatus = null,
    BetlistOutputParams betlistOutputParams = null,
    bool showFooter = false,
    bool showOrder = false) : BetListBaseViewModel(message, betListData, builder, ticketHelper, showMemberName)
{
    public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

    public bool ShowContributionAmountRoundNote { get; } = showContributionAmountRoundNote;

    public bool IsGroupHeadline { get; } = isGroupHeadline;

    public Footer Footer { get; } = new Footer(betListData, message.ViewerLevel, showMemberName, excludingTicketStatus);

    public BetlistOutputParams BetlistOutputParams { get; } = betlistOutputParams ?? new BetlistOutputParams();

    public bool ShowFooter { get; } = showFooter;

    public bool ShowOrder { get; } = showOrder;
}