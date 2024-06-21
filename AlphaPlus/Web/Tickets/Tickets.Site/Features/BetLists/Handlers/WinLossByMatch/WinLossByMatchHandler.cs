using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.BetLists.WinLossByMatch;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLossByMatch;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLossByMatch;

public class WinLossByMatchHandler(
    IWinLossByMatchService winLossByMatchService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<WinLossByMatchQuery, WinLossByMatchViewModel>
{
    private readonly IWinLossByMatchService winLossByMatchService = winLossByMatchService;

    public async Task<WinLossByMatchViewModel> Handle(WinLossByMatchQuery message, CancellationToken cancellationToken)
    {
        var param = new WinLossByMatchParam(
            message.CustId,
            message.LeagueId,
            message.MatchId,
            message.BetType,
            message.Race,
            message.LiveIndicator == 1,
            message.Language,
            message.CustName,
            message.FromDate,
            message.ToDate,
            message.CustLevelId,
            message.DisplayVNIP,
            message.Site);
        var betlistData = await winLossByMatchService.GetBetList(param);

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new WinLossByMatchViewModel(message, betlistData, Builder, TicketHelper);
    }
}