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

public class WinLossHandler(
    IOptions<AppSettings> options,
    IProductService productService,
    IWinLossService winLossService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IWebHostEnvironment webHostEnvironment) :
        WinLossBaseHandler(options, productService, winLossService, builder, excelBuilder, ticketHelper, monitorService, ipService, webHostEnvironment),
        IRequestHandler<WinLossQuery, WinLossViewModel>
{
    public Task<WinLossViewModel> Handle(WinLossQuery message, CancellationToken cancellationToken)
        => GetBetList(message, appSettings.BetListMaxTotalRowView);
}
