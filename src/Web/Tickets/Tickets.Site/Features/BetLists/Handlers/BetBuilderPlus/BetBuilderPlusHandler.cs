using AlphaPlus.BetLists.BetBuilderPlus;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilderPlus;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetBuilder;

public class BetBuilderPlusHandler(
    IBetBuilderPlusService betBuilderPlusService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    IPService ipService,
    IOptions<AppSettings> options) :
      BetListBaseHandler(builder, ticketHelper, null, ipService, options),
      IRequestHandler<BetBuilderPlusQuery, string>
{
    private readonly IBetBuilderPlusService betBuilderPlusService = betBuilderPlusService;

    public async Task<string> Handle(BetBuilderPlusQuery message, CancellationToken cancellationToken)
    {
        return await betBuilderPlusService.GetBetBuilderPlusTicketsDetailsHtml(ticketHelper, message.TransId, message.CustId, message.TransDesc);
    }
}
