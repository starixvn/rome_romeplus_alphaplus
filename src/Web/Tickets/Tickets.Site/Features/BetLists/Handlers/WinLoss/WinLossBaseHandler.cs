using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.BetLists.WinLoss;
using AlphaPlus.Core.Extensions;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLoss;

public class WinLossBaseHandler(
    IProductService productService,
    IWinLossService winLossService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    AllowedSitesToShareLoad allowedSitesToShareLoad,
    IWebHostEnvironment webHostEnvironment)
        : BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment)
{
    protected readonly AppSettings appSettings = options.Value;
    private readonly AllowedSitesToShareLoad allowedSitesToShareLoad = allowedSitesToShareLoad;
    private readonly IProductService productService = productService;
    private readonly IWinLossService winLossService = winLossService;

    protected async Task<WinLossViewModel> GetBetList(WinLossQuery message, int maxTotalRow)
    {
        var products = await GetProducts(message);
        var productViewModel = BuildProductViewModel(products, message.ProductIds, message.Language);
        string productDetailsXml;
        if (appSettings.Is789Y)
        {
            productDetailsXml = await productService.BuildProductDetailsXml789y(
                products,
                message.ProductIds,
                message.ResourceIds,
                appSettings.ProductOfferProductDetail);
        }
        else
        {
            var productDetails = await GetProductDetails(message, products);
            productDetailsXml = productService.BuildProductDetailsXml(productDetails);
        }

        var showContributionAmountRoundNote = message.IsGroupHeadline
            ? await productService.IsEnabledJackpot(message.Site, message.CurrencyId, appSettings.LocalTest)
            : message.IsEnabledJackpot;

        var param = new WinLossParam(
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo).ToStartOfDay(),
            message.ToDate,
            productDetailsXml,
            message.DisplayVNIP,
            message.SuperId,
            message.Site,
            message.IsExcel,
            maxTotalRow,
            maxTotalRow,
            allowedSitesToShareLoad.GetIP(message.Site));
        var betlistData = await winLossService.GetBetList(param);

        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new WinLossViewModel(message, betlistData, Builder, TicketHelper, showContributionAmountRoundNote, message.IsGroupHeadline, showFooter: true);
    }

    private async Task<IEnumerable<Product>> GetProducts(WinLossQuery message)
    {
        IEnumerable<Product> products;
        if (message.IsGroupHeadline)
        {
            var productByHLParam = new GetProductsByGroupHeadlineParam(message.ViewerId, appSettings.SettingStatus, message.MinDate, appSettings.IsSiteLabel777);
            products = await productService.GetProductsByGroupHeadline(productByHLParam);
        }
        else
        {
            var productParam = new GetProductsParam(message.Site, message.CurrencyId, appSettings.SettingStatus, message.Language, message.MinDate, appSettings.IsSiteLabel777);
            products = await productService.GetProducts(productParam);
        }

        return products;
    }

    private async Task<IEnumerable<ProductDetail>> GetProductDetails(WinLossQuery message, IEnumerable<Product> products)
    {
        var param = new GetProductDetailsParam(message.CustId, appSettings.ProductOfferProductDetail, appSettings.IsSiteLabel777);
        var productDetails = await productService.GetProductDetails(param, products);

        if ("*" != message.ResourceIds)
        {
            var userSelectedResourceIds = message.ResourceIds.ToListInt().ToList();
            productDetails = productDetails.Where(p => userSelectedResourceIds.Contains(p.ResourceId));
        }

        return productDetails;
    }

    private async Task<IEnumerable<ProductDetail>> GetProductDetails789y(WinLossQuery message, IEnumerable<Product> products)
    {
        var param = new GetProductDetailsParam(message.CustId, appSettings.ProductOfferProductDetail, appSettings.IsSiteLabel777);
        var productDetails = await productService.GetProductDetails(param, products);

        if ("*" != message.ResourceIds)
        {
            var userSelectedResourceIds = message.ResourceIds.ToListInt().ToList();
            productDetails = productDetails.Where(p => userSelectedResourceIds.Contains(p.ResourceId));
        }

        return productDetails;
    }
}