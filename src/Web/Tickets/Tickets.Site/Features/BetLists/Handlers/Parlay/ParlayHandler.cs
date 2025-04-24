using AlphaPlus.BetLists.Parlay;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.Parlay;

public class ParlayHandler(
    IParlayService parlayService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    IPService ipService,
    IOptions<AppSettings> options,
    IBrowserHelper browserHelper) :
        BetListBaseHandler(builder, ticketHelper, null, ipService, options),
        IRequestHandler<MixParlayQuery, MixParlayViewModel>,
        IRequestHandler<ComboParlayQuery, ComboParlayViewModel>,
        IRequestHandler<ComboParlayDetailQuery, ComboParlayDetailViewModel>
{
    private readonly IParlayService parlayService = parlayService;
    private readonly IBrowserHelper browserHelper = browserHelper;

    public async Task<MixParlayViewModel> Handle(MixParlayQuery message, CancellationToken cancellationToken)
    {
        var tickets = await parlayService.GetParlayBetList(message.TransId, message.CustId);

        return new MixParlayViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
    }

    public async Task<ComboParlayViewModel> Handle(ComboParlayQuery message, CancellationToken cancellationToken)
    {
        var tickets = await parlayService.GetParlayBetList(message.TransId, message.CustId);

        return new ComboParlayViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
    }

    public async Task<ComboParlayDetailViewModel> Handle(ComboParlayDetailQuery message, CancellationToken cancellationToken)
    {
        var comboParlayDetailData = await parlayService.GetComboParlayDetail(message.RefNo, message.CustId, message.Language);

        return new ComboParlayDetailViewModel(message, comboParlayDetailData, TicketHelper, browserHelper.IsMobile(false), isExportingExcel: false);
    }
}