using AlphaPlus.BetLists.BetsForecast;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetTypeAndMatch;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetTypeAndMatch;

public class BetListByBetTypeAndMatchHandler(
    IRunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService,
    IForecastService forecastService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<BetListByBetTypeAndMatchQuery, BetListByBetTypeAndMatchViewModel>
{
    private readonly IForecastService forecastService = forecastService;
    private readonly IRunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService = runningBetListByBetTypeAndMatchService;

    public async Task<BetListByBetTypeAndMatchViewModel> Handle(BetListByBetTypeAndMatchQuery message, CancellationToken cancellationToken)
    {
        var param = CreateRunningBetListByBetTypeAndMatchParam(message);

        var betListData = await runningBetListByBetTypeAndMatchService.GetBetList(param);
        var supportScoreMapBetTypeIds = betListData.Total == 0
            ? Enumerable.Empty<int>()
            : await forecastService.GetBetTypeIdsSupportScoreMap();

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betListData.Tickets);

        return new BetListByBetTypeAndMatchViewModel(
            message,
            betListData,
            Builder,
            TicketHelper,
            supportScoreMapBetTypeIds);
    }

    protected internal static RunningBetListByBetTypeAndMatchParam CreateRunningBetListByBetTypeAndMatchParam(BetListByBetTypeAndMatchQuery message)
        => new RunningBetListByBetTypeAndMatchParam(
            message.MatchId,
            message.ViewerId,
            message.BetType,
            message.Language,
            message.IsLive,
            message.BetTeam,
            message.ViewerLevelId,
            message.DisplayVNIP);
}