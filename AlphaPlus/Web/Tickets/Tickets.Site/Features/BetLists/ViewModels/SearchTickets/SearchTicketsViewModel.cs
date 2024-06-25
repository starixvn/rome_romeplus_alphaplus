using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.SearchTickets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.SearchTickets;

public class SearchTicketsViewModel(
    SearchTicketsQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : WinLossViewModel(message, betListData, builder, ticketHelper, message.IsEnabledJackpot, false, true)
{
    protected override string GetTitle() => Resources.SearchTickets;

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReportWithFormat, message.MinDate.ToShortDateWithSlash());
}