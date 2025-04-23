using AlphaPlus.BetLists.MoveStatementHistory;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.MoveStatementHistory;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.MoveStatementHistory;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.MoveStatementHistory;

public class MoveStatementHistoryHandler(
    IMoveStatementHistoryService moveStatementHistoryService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<MoveStatementHistoryQuery, MoveStatementHistoryViewModel>
{
    private readonly IMoveStatementHistoryService moveStatementHistoryService = moveStatementHistoryService;

    public async Task<MoveStatementHistoryViewModel> Handle(MoveStatementHistoryQuery message, CancellationToken cancellationToken)
    {
        var param = new MoveStatementHistoryParam(message.CustId, message.TransId, message.DisplayVNIP, message.Site);
        var tickets = await moveStatementHistoryService.GetBetList(param);

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

        return new MoveStatementHistoryViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
    }
}
