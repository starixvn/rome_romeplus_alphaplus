using AlphaPlus.BetLists.JackpotDetails;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.JackpotDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.JackpotDetails;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.JackpotDetails;

public class JackpotDetailsHandler(
    IJackpotDetailsService jackpotDetailsService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<JackpotDetailsQuery, JackpotDetailsViewModel>
{
    private readonly IJackpotDetailsService jackpotDetailsService = jackpotDetailsService;

    public async Task<JackpotDetailsViewModel> Handle(JackpotDetailsQuery message, CancellationToken cancellationToken)
    {
        var param = CreateJackpotDetailsParam(message);
        var tickets = await jackpotDetailsService.GetBetList(param);

        return new JackpotDetailsViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
    }

    protected internal static JackpotDetailsParam CreateJackpotDetailsParam(JackpotDetailsQuery message)
        => new JackpotDetailsParam(message.TransId, message.CustId, message.BetType);
}