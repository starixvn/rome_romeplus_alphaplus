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
    BetListOutputParams outputParams = null,
    bool showFooter = false,
    bool showOrder = false,
    int startIndex = 1) : BetListBaseViewModel(message, betListData, builder, ticketHelper, showMemberName)
{
    public bool AllowToViewSuperPT => ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT => ViewerLevel.IsMasterOrSuperLevel();

    public bool ShowContributionAmountRoundNote { get; } = showContributionAmountRoundNote;

    public bool IsGroupHeadline { get; } = isGroupHeadline;

    public Footer Footer { get; } = showFooter ? new Footer(betListData, message.ViewerLevel, showMemberName, excludingTicketStatus) : null;

    public BetListOutputParams OutputParams { get; } = outputParams ?? new BetListOutputParams();

    public bool ShowOrder { get; } = showOrder;

    public int StartIndex { get; } = startIndex;
}