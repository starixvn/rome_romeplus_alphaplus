using AlphaPlus.BetLists.RiskAnalysisIPMatching;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RiskAnalysisIPMatching;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RiskAnalysisIPMatching;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.RiskAnalysisIPMatching;

public class RiskAnalysisIPMatchingHandler(
    IRiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<RiskAnalysisIPMatchingQuery, RiskAnalysisIPMatchingViewModel>
{
    private readonly IRiskAnalysisIPMatchingService riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;

    public async Task<RiskAnalysisIPMatchingViewModel> Handle(RiskAnalysisIPMatchingQuery message, CancellationToken cancellationToken)
    {
        var param = CreateIPMatchingParamParam(message);
        var betlistData = await riskAnalysisIPMatchingService.GetBetList(param);

        return new RiskAnalysisIPMatchingViewModel(message, betlistData, Builder, TicketHelper);
    }

    protected internal static RiskAnalysisIPMatchingParam CreateIPMatchingParamParam(RiskAnalysisIPMatchingQuery message)
        => new RiskAnalysisIPMatchingParam(message.CustId, message.IP, message.Date, message.DisplayVNIP, message.SuperId, message.Site);
}