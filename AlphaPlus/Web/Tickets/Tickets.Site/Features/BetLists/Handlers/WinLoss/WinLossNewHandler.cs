using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.BetLists.WinLoss;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLoss;

public class WinLossNewHandler(
    IOptions<AppSettings> options,
    IProductService productService,
    IWinLossService winLossService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<WinLossNewQuery, WinLossNewViewModel>
{
    private readonly AppSettings appSettings = options.Value;
    private readonly IProductService productService = productService;
    private readonly IWinLossService winLossService = winLossService;

    public Task<WinLossNewViewModel> Handle(WinLossNewQuery message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}