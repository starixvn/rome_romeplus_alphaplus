using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.MoveStatementHistory;

public class MoveStatementHistoryViewModel(
    BetListBaseQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : BetListBaseViewModel(message, betListData, builder, ticketHelper)
{
    public bool AllowToViewSuperPT => ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT => ViewerLevel.IsMasterOrSuperLevel();

    public string GetMoveStatementHistoryTitle
        => $"{CustName.ToUpperInvariant()} - {Resources.TicketDetails.ToUpperInvariant()}";
}
