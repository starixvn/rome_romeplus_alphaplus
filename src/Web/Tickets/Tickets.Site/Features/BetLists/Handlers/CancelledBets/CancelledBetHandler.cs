using AlphaPlus.BetLists.CancelledBets;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CancelledBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CancelledBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CancelledBets;

public class CancelledBetHandler(
    ICancelledBetService cancelledBetService,
    IProductService productService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<CancelledBetQuery, CancelledBetViewModel>
{
    private static readonly HashSet<int> SupportedProducts =
    [
        ProductIdConstants.Sportsbook,
        ProductIdConstants.NumberGame,
        ProductIdConstants.VirtualSports,
        ProductIdConstants.VirtualSports2,
        ProductIdConstants.Keno,
        ProductIdConstants.GoldDeluxe,
        ProductIdConstants.Lottery,
        ProductIdConstants.TableGames,
        ProductIdConstants.VirtualGames,
        ProductIdConstants.SportsLottery,
        ProductIdConstants.Bitcoin
    ];

    private readonly AppSettings appSettings = options.Value;
    private readonly ICancelledBetService cancelledBetService = cancelledBetService;
    private readonly IProductService productService = productService;

    public async Task<CancelledBetViewModel> Handle(CancelledBetQuery message, CancellationToken cancellationToken)
    {
        var productViewModel = BuildProductViewModel(await GetProducts(message), message.ProductIds, message.Language);
        var param = new CancelledBetParam(
            message.ViewerId,
            message.FromDate,
            message.ToDate,
            message.ViewerLevelId,
            productViewModel.UserSelectedProductIds,
            message.StatusIds,
            message.PageIndex,
            message.PageSize,
            message.DisplayVNIP,
            message.SuperId,
            message.Site);
        var betListData = await cancelledBetService.GetBetList(param);

        return new CancelledBetViewModel(
            message,
            betListData.DataSource.FirstOrDefault(),
            new PagerViewModel(betListData.PageIndex, betListData.PageSize, betListData.TotalRecords),
            productViewModel,
            Builder,
            TicketHelper);
    }

    private async Task<IEnumerable<Product>> GetProducts(CancelledBetQuery message)
    {
        var param = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate,
            appSettings.IsSiteLabel777);
        var products = await productService.GetProducts(param);

        return products.Where(p => SupportedProducts.Contains(p.Id));
    }
}