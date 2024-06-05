using AlphaPlus.BetLists.Running;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetType;

public class BetListByBetTypeHandler(
    IRunningBetListByBetTypeService runningBetListByBetTypeService,
    IForecastService forecastService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService), IRequestHandler<BetListByBetTypeQuery, BetListByBetTypeViewModel>
{
    private readonly IForecastService forecastService = forecastService;
    private readonly IRunningBetListByBetTypeService runningBetListByBetTypeService = runningBetListByBetTypeService;

    public async Task<BetListByBetTypeViewModel> Handle(BetListByBetTypeQuery message, CancellationToken cancellationToken)
    {
        var param = new RunningBetListByBetTypeParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.BetType,
            message.DisplayVNIP,
            message.Site);
        var tickets = await runningBetListByBetTypeService.GetBetList(param);
        var supportScoreMapBetTypeIds = tickets.Any()
            ? await forecastService.GetBetTypeIdsSupportScoreMap()
            : [];
        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

        return new BetListByBetTypeViewModel(
            message,
            new BetListData(tickets, null),
            Builder,
            TicketHelper,
            supportScoreMapBetTypeIds);
    }
}