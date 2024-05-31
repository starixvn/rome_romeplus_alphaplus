using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.SearchTickets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.SearchTickets;

public class SearchTicketsViewModel : WinLossViewModel
{
    public SearchTicketsViewModel(
        SearchTicketsQuery query,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper) : base(query, betListData, builder, ticketHelper, query.IsEnabledJackpot, false, true)
    {
        ViewDataInstructionMessage = string.Format(
            Resources.CanOnlyViewDataAfterTimeHistoryReportWithFormat,
            query.MinDate.ToShortDateWithSlash());
        Title = Resources.SearchTickets;
    }

    public string ViewDataInstructionMessage { get; }
}