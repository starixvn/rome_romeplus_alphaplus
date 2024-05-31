using AlphaPlus.BetLists.SearchTickets;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.SearchTickets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.SearchTickets;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.SearchTickets;

public class SearchTicketsHandler(
    ISearchTicketsService searchTicketsService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<SearchTicketsQuery, SearchTicketsViewModel>
{
    private readonly ISearchTicketsService searchTicketsService = searchTicketsService;

    public async Task<SearchTicketsViewModel> Handle(SearchTicketsQuery message, CancellationToken cancellationToken)
    {
        var param = new SearchTicketsParam(message.RefNo, message.ViewerId, message.ViewerLevelId, message.DisplayVNIP, message.Site);
        var betListData = await searchTicketsService.GetBetList(param);
        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betListData.Tickets);

        return new SearchTicketsViewModel(message, betListData, Builder, TicketHelper);
    }
}