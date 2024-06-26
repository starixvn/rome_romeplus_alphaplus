using AlphaPlus.Shared.AppResources.Betlist.Services;
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
    public bool AllowToViewSuperPT { get; } = message.ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT { get; } = message.ViewerLevel.IsMasterOrSuperLevel();

    public string GetMoveStatementHistoryTitle
        => $"{CustName.ToUpperInvariant()} - {Resources.TicketDetails.ToUpperInvariant()}";
}
