using AlphaPlus.BetLists.CorrectScore;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CorrectScore;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CorrectScore;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CorrectScore;

public class CorrectScoreHandler(
    ICorrectScoreService correctScoreService,
    IForecastService forecastService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<CorrectScoreQuery, CorrectScoreViewModel>
{
    private readonly ICorrectScoreService correctScoreService = correctScoreService;
    private readonly IForecastService forecastService = forecastService;

    public async Task<CorrectScoreViewModel> Handle(CorrectScoreQuery message, CancellationToken cancellationToken)
    {
        var param = new CorrectScoreParam(message.ViewerId, message.ViewerLevelId, message.DisplayVNIP, message.Site);

        var tickets = await correctScoreService.GetBetList(param);
        var supportScoreMapBetTypeIds = tickets.Any()
            ? await forecastService.GetBetTypeIdsSupportScoreMap()
            : [];

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

        return new CorrectScoreViewModel(message, new BetListData(tickets, null), Builder, TicketHelper, supportScoreMapBetTypeIds);
    }
}
