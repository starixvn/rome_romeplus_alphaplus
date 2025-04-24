using AlphaPlus.BetLists.RunningBetListByBetType;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CombinationMixParlay;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CombinationMixParlay;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CombinationMixParlay;

public class CombinationMixParlayHandler(
    IRunningBetListByBetTypeService runningBetListByBetTypeService,
    IForecastService forecastService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<CombinationMixParlayQuery, CombinationMixParlayViewModel>
{
    private readonly IForecastService forecastService = forecastService;
    private readonly IRunningBetListByBetTypeService runningBetListByBetTypeService = runningBetListByBetTypeService;

    public async Task<CombinationMixParlayViewModel> Handle(CombinationMixParlayQuery message, CancellationToken cancellationToken)
    {
        var param = BetListByBetTypeHandler.CreateRunningBetListByBetTypeParam(message);

        var tickets = await runningBetListByBetTypeService.GetBetList(param);
        var supportScoreMapBetTypeIds = tickets.Any()
            ? await forecastService.GetBetTypeIdsSupportScoreMap()
            : [];

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

        return new CombinationMixParlayViewModel(message, new BetListData(tickets, null), Builder, TicketHelper, supportScoreMapBetTypeIds);
    }
}
