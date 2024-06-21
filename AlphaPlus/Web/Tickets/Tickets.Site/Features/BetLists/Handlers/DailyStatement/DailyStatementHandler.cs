using AlphaPlus.BetLists.DailyStatement;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.DailyStatement;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.DailyStatement;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.DailyStatement;

public class DailyStatementHandler(
    IDailyStatementService dailyStatementService,
    IForecastService forecastService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<DailyStatementQuery, DailyStatementViewModel>
{
    private readonly IDailyStatementService dailyStatementService = dailyStatementService;
    private readonly IForecastService forecastService = forecastService;

    public async Task<DailyStatementViewModel> Handle(DailyStatementQuery message, CancellationToken cancellationToken)
    {
        var param = new DailyStatementParam(
            message.CustId,
            message.WinLossDate,
            message.TransType,
            message.DisplayVNIP,
            message.Site);
        var betlistData = await dailyStatementService.GetBetList(param);
        var supportScoreMapBetTypeIds = betlistData.Total == 0
            ? []
            : await forecastService.GetBetTypeIdsSupportScoreMap();

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new DailyStatementViewModel(message, betlistData, Builder, TicketHelper, supportScoreMapBetTypeIds);
    }
}
