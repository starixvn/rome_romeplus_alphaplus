using AlphaPlus.BetLists.RunningBetList;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Extensions;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RunningBetList;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RunningBetList;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.RunningBetList;

public class RunningBetListHandler(
    IAutoFollowBetService autoFollowBetService,
    IRunningBetListService runningBetListService,
    IProductService productService,
    IForecastService forecastService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<RunningBetListQuery, RunningBetListViewModel>
{
    private const int SabaPromotionProductId = 37;
    private readonly AppSettings appSettings = options.Value;
    private readonly IAutoFollowBetService autoFollowBetService = autoFollowBetService;
    private readonly IRunningBetListService runningBetListService = runningBetListService;
    private readonly IProductService productService = productService;
    private readonly IForecastService forecastService = forecastService;

    public async Task<RunningBetListViewModel> Handle(RunningBetListQuery message, CancellationToken cancellationToken)
    {
        if (message.AutoFollowBetIsDummyAgent)
        {
            message.CustId = await autoFollowBetService.GetRealCustId(message.ViewerId, message.CustId);
        }

        var products = GetProducts(message);
        var productViewModel = BuildProductViewModel(products, message.ProductIds, message.Language);
        var param = new RunningBetListParam(
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo).ToStartOfDay(),
            message.ToDate.ToEndOfDay(),
            message.GetAll,
            message.CustName,
            message.DisplayVNIP,
            message.Site);
        var betlistData = await runningBetListService.GetBetList(param);
        var supportScoreMapBetTypeIds = betlistData.Tickets.Any()
            ? await forecastService.GetBetTypeIdsSupportScoreMap()
            : [];

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new RunningBetListViewModel(
            message,
            betlistData,
            productViewModel,
            supportScoreMapBetTypeIds,
            Builder,
            TicketHelper);
    }

    private IEnumerable<Product> GetProducts(RunningBetListQuery message)
    {
        var param = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate,
            appSettings.IsSiteLabel777);

        return productService.GetProducts(param).Where(p => p.Id != SabaPromotionProductId);
    }
}
