using AlphaPlus.BetLists.CasinoDetails;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CasinoDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CasinoDetails;

public class CasinoDetailsHandler(
    ICasinoDetailsService casinoDetailsService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<CasinoDetailsQuery, CasinoDetailsViewModel>
{
    private readonly ICasinoDetailsService casinoDetailsService = casinoDetailsService;

    public async Task<CasinoDetailsViewModel> Handle(CasinoDetailsQuery message, CancellationToken cancellationToken)
    {
        var param = new CasinoDetailsParam(
            message.RefNo,
            message.CustId,
            message.TransType,
            message.ProductId,
            message.PageIndex,
            message.PageSize,
            message.DisplayVNIP);
        var pagedBetListData = await casinoDetailsService.GetBetList(param);

        return new CasinoDetailsViewModel(
            message,
            new BetListData(pagedBetListData.DataSource, null),
            new PagerViewModel(pagedBetListData.PageIndex, pagedBetListData.PageSize, pagedBetListData.TotalRecords),
            Builder,
            TicketHelper);
    }
}
