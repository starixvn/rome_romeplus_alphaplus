using AlphaPlus.BetLists.MemberBetList;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Extensions;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.MemberBetList;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.MemberBetList;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.MemberBetList;

public class MemberBetListHandler(
    IMemberBetListService memberBetListService,
    IProductService productService,
    IForecastService forecastService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<MemberBetListQuery, MemberBetListViewModel>
{
    private readonly AppSettings appSettings = options.Value;
    private readonly IMemberBetListService memberBetListService = memberBetListService;
    private readonly IProductService productService = productService;
    private readonly IForecastService forecastService = forecastService;

    public async Task<MemberBetListViewModel> Handle(MemberBetListQuery message, CancellationToken cancellationToken)
    {
        var products = await GetProducts(message);
        var productViewModel = BuildProductViewModel(products, message.ProductIds, message.Language);
        var param = new MemberBetListParam(
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo).ToStartOfDay(),
            message.ToDate.ToEndOfDay(),
            message.CustName,
            message.DisplayVNIP,
            message.SuperId,
            message.Site);
        var betlistData = await memberBetListService.GetBetList(param);
        var supportScoreMapBetTypeIds = betlistData.Tickets.Any()
            ? await forecastService.GetBetTypeIdsSupportScoreMap()
            : [];

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new MemberBetListViewModel(
            message,
            betlistData,
            productViewModel,
            supportScoreMapBetTypeIds,
            Builder,
            TicketHelper);
    }

    private Task<IEnumerable<Product>> GetProducts(MemberBetListQuery message)
    {
        var param = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate,
            appSettings.IsSiteLabel777);

        return productService.GetProducts(param);
    }
}