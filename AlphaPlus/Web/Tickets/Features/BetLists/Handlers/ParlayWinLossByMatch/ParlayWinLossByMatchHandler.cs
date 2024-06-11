using AlphaPlus.BetLists.ParlayWinLossByMatch;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.ParlayWinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.ParlayWinLossByMatch;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.ParlayWinLossByMatch;

public class ParlayWinLossByMatchHandler(
    IParlayWinLossByMatchService parlayWinLossByMatchService,
    ISmartPunterBetListMonitor monitorService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<ParlayWinLossByMatchQuery, ParlayWinLossByMatchViewModel>
{
    private readonly IParlayWinLossByMatchService parlayWinLossByMatchService = parlayWinLossByMatchService;

    public async Task<ParlayWinLossByMatchViewModel> Handle(ParlayWinLossByMatchQuery message, CancellationToken cancellationToken)
    {
        var param = new ParlayWinLossByMatchParam(
            message.CustId,
            message.BetType,
            message.LangKey,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.CustLevelId,
            message.DisplayVNIP,
            message.Site);
        var tickets = await parlayWinLossByMatchService.GetBetList(param);

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

        return new ParlayWinLossByMatchViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
    }
}