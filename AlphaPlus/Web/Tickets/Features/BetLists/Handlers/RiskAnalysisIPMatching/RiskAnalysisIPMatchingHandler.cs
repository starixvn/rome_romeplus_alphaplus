using AlphaPlus.BetLists.RiskAnalysisIPMatching;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RiskAnalysisIPMatching;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RiskAnalysisIPMatching;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.RiskAnalysisIPMatching;

public class RiskAnalysisIPMatchingHandler(
    IRiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<RiskAnalysisIPMatchingQuery, RiskAnalysisIPMatchingViewModel>
{
    private readonly IRiskAnalysisIPMatchingService riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;

    public async Task<RiskAnalysisIPMatchingViewModel> Handle(RiskAnalysisIPMatchingQuery message, CancellationToken cancellationToken)
    {
        var param = new RiskAnalysisIPMatchingParam(
            message.CustId,
            message.IP,
            message.Date,
            message.DisplayVNIP,
            message.Site);
        var betlistData = await riskAnalysisIPMatchingService.GetBetList(param);

        return new RiskAnalysisIPMatchingViewModel(message, betlistData, Builder, TicketHelper);
    }
}