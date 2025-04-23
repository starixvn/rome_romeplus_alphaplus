using AlphaPlus.BetLists.WinLossByMatch;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLossByMatch;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLossByMatch;

public class WinLossByMatchHandler(
    IWinLossByMatchService winLossByMatchService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<WinLossByMatchQuery, WinLossByMatchViewModel>
{
    private readonly IWinLossByMatchService winLossByMatchService = winLossByMatchService;

    public async Task<WinLossByMatchViewModel> Handle(WinLossByMatchQuery message, CancellationToken cancellationToken)
    {
        var param = CreateWinLossParam(message);
        var betlistData = await winLossByMatchService.GetBetList(param);

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new WinLossByMatchViewModel(message, betlistData, Builder, TicketHelper);
    }

    protected internal static WinLossByMatchParam CreateWinLossParam(WinLossByMatchQuery message)
        => new WinLossByMatchParam(
            message.CustId,
            message.LeagueId,
            message.MatchId,
            message.LeagueType,
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
}