using System.Text;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.BetLists.WinLoss;
using AlphaPlus.Core.Extensions;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLoss;

public class WinLossNewHandler(
    IProductService productService,
    IWinLossService winlossService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    AllowedSitesToShareLoad allowedSitesToShareLoad) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService, options),
        IRequestHandler<WinLossNewQuery, WinLossNewViewModel>,
        IRequestHandler<WinLossWeekSelectionQuery, WinLossWeekSelectionViewModel>,
        IRequestHandler<WinLossWeeksQuery, IEnumerable<WinLossWeekViewModel>>,
        IRequestHandler<WinLossBetListQuery, WinLossBetListViewModel>
{
    private readonly AppSettings appSettings = options.Value;
    private readonly AllowedSitesToShareLoad allowedSitesToShareLoad = allowedSitesToShareLoad;
    private readonly IProductService productService = productService;
    private readonly IWinLossService winlossService = winlossService;

    public async Task<WinLossNewViewModel> Handle(WinLossNewQuery message, CancellationToken cancellationToken)
    {
        var showContributionAmountRoundNote = message.IsEnabledJackpot;
        var (productViewModel, productDetailsXml) = await BuildProductViewModel(message, message.ProductIds, message.ResourceIds);
        var weekDataParam = new WinLossWeekParam(
            message.ViewerId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            productViewModel.UserSelectedProductIds,
            productDetailsXml,
            message.CustName,
            allowedSitesToShareLoad.GetIP(message.Site));
        var weekData = await winlossService.GetWeekData(weekDataParam);
        var betlistData = new BetListData([], []);

        if (weekData.Any())
        {
            var param = new WinLossParam(
                message.CustId,
                productViewModel.UserSelectedProductIds,
                (message.WeekFromDate ?? message.FromDate).GetGreaterDate(message.LimitedDateToShowInfo),
                message.WeekToDate ?? message.ToDate,
                productDetailsXml,
                message.DisplayVNIP,
                message.SuperId,
                message.Site,
                maxTotalRow: appSettings.BetListMaxTotalRowView,
                maxTotalRowConfig: appSettings.BetListMaxTotalRowView,
                ip: allowedSitesToShareLoad.GetIP(message.Site));
            betlistData = await winlossService.GetBetList(param);
            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);
        }

        return new WinLossNewViewModel(
            message,
            productViewModel,
            BuildWeekViewModels(weekData, message.FromDate, message.ToDate, message.WeekFromDate, message.WeekToDate),
            betlistData,
            Builder,
            TicketHelper,
            showContributionAmountRoundNote,
            message.IsGroupHeadline);
    }

    private async Task<(ProductMultiSelectionViewModel, string)> BuildProductViewModel(
        WinLossQuery message,
        string userSelectedProductIds,
        string userSelectedResourceIds)
    {
        var products = await GetProducts(message);
        var productDetails = await GetProductDetails(message.ViewerId, products);
        var (productGroups, selectedProductIds) = BuildProductGroups(products, userSelectedProductIds);
        var (productDetailDTOList, validUserSelectedResourceIds, validUserSelectedProductIds, productDetailXml) = BuildProductDetailGroups(
            productDetails,
            selectedProductIds,
            userSelectedResourceIds);
        if (options.Value.Is789Y)
        {
            productDetailXml = await productService.BuildProductDetailsXml789y(
                products,
                message.ProductIds,
                message.ResourceIds,
                options.Value.ProductOfferProductDetail);
        }

        return (new ProductMultiSelectionViewModel(
            productGroups,
            validUserSelectedProductIds.JoinWithComma(),
            message.Language,
            productDetailDTOList,
            validUserSelectedResourceIds.JoinWithComma()), productDetailXml);
    }

    private static (IEnumerable<ProductGroupDTO>, IEnumerable<int>) BuildProductGroups(
        IEnumerable<Product> products,
        string userSelectedProductIds)
    {
        var productGroups = new List<ProductGroupDTO>();
        var validUserSelectedProductIds = new List<int>();

        var isCheckedAll = "*" == userSelectedProductIds;
        var userSelectedProductIdsList = isCheckedAll ? [] : userSelectedProductIds.ToListInt().ToList();

        var offeredProductGroups = products.GroupBy(p => new { p.Type, p.TypeName });
        foreach (var groups in offeredProductGroups)
        {
            var subProductsCheckedCount = 0;
            var subProducts = groups.Select(item =>
            {
                var isChecked = isCheckedAll || userSelectedProductIdsList.Contains(item.Id);
                if (isChecked)
                {
                    subProductsCheckedCount++;
                    validUserSelectedProductIds.Add(item.Id);
                }

                return new ProductGroupDTO(item.Id, item.Name, isChecked, null);
            }).ToList();

            if (groups.Key.Type == 0)
            {
                productGroups.AddRange(subProducts);
            }
            else
            {
                productGroups.Add(new ProductGroupDTO(
                    groups.Key.Type,
                    groups.Key.TypeName,
                    isCheckedAll || subProductsCheckedCount == subProducts.Count,
                    subProducts));
            }
        }

        return (productGroups, validUserSelectedProductIds);
    }

    private (IEnumerable<ProductDetailDTO>, IEnumerable<int>, IEnumerable<int>, string) BuildProductDetailGroups(
        IEnumerable<ProductDetail> productDetails,
        IEnumerable<int> validUserSelectedProductIds,
        string userSelectedResourceIds)
    {
        var isCheckedAll = "*" == userSelectedResourceIds;
        var userSelectedResourceIdsList = isCheckedAll ? [] : userSelectedResourceIds.ToListInt().ToList();

        var resourceIdHashSet = new HashSet<int>();
        var productDetailDTOList = new List<ProductDetailDTO>();
        var validUserSelectedResourceIds = new HashSet<int>();
        var allProductsSupportProductDetails = new HashSet<int>();
        var selectedProductIds = new HashSet<int>();

        var xml = new StringBuilder();
        foreach (var item in productDetails)
        {
            var isChecked = isCheckedAll || (validUserSelectedProductIds.Contains(item.ProductId) && userSelectedResourceIdsList.Contains(item.ResourceId));
            if (isChecked)
            {
                validUserSelectedResourceIds.Add(item.ResourceId);
                selectedProductIds.Add(item.ProductId);
                xml.Append(item.ToXml());
            }

            if (resourceIdHashSet.Add(item.ResourceId))
            {
                productDetailDTOList.Add(new ProductDetailDTO(item.ProductId, item.ResourceId, item.DisplayName, isChecked));
            }

            allProductsSupportProductDetails.Add(item.ProductId);
        }

        // Get un-selected Product Ids
        var unselectedProductIds = allProductsSupportProductDetails.Except(selectedProductIds).ToList();
        if (unselectedProductIds.Any())
        {
            validUserSelectedProductIds = validUserSelectedProductIds.Except(unselectedProductIds);
        }

        return (productDetailDTOList, validUserSelectedResourceIds, validUserSelectedProductIds.ToList(), $"<ROOT>{xml}</ROOT>");
    }

    private Task<IEnumerable<Product>> GetProducts(WinLossQuery message)
    {
        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate,
            appSettings.IsSiteLabel777);

        return productService.GetProducts(productParam);
    }

    private Task<IEnumerable<ProductDetail>> GetProductDetails(int custId, IEnumerable<Product> products)
    {
        var param = new GetProductDetailsParam(custId, appSettings.ProductOfferProductDetail, appSettings.IsSiteLabel777);

        return productService.GetProductDetails(param, products);
    }

    public async Task<WinLossWeekSelectionViewModel> Handle(WinLossWeekSelectionQuery message, CancellationToken cancellationToken)
    {
        var showContributionAmountRoundNote = message.IsEnabledJackpot;
        var (productViewModel, productDetailsXml) = await BuildProductViewModel(message, message.ProductIds, message.ResourceIds);
        if (options.Value.Is789Y)
        {
            productDetailsXml = await productService.BuildProductDetailsXml789y(
                await GetProducts(message),
                message.ProductIds,
                message.ResourceIds,
                options.Value.ProductOfferProductDetail);
        }

        var param = new WinLossParam(
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            productDetailsXml,
            message.DisplayVNIP,
            message.SuperId,
            message.Site,
            maxTotalRow: appSettings.BetListMaxTotalRowView,
            maxTotalRowConfig: appSettings.BetListMaxTotalRowView,
            ip: allowedSitesToShareLoad.GetIP(message.Site));
        var betlistData = await winlossService.GetBetList(param);
        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new WinLossWeekSelectionViewModel(
            message,
            betlistData,
            Builder,
            TicketHelper,
            showContributionAmountRoundNote,
            message.IsGroupHeadline);
    }

    public async Task<IEnumerable<WinLossWeekViewModel>> Handle(WinLossWeeksQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, productDetailsXml) = await BuildProductViewModel(message, message.ProductIds, message.ResourceIds);
        if (options.Value.Is789Y)
        {
            productDetailsXml = await productService.BuildProductDetailsXml789y(
                await GetProducts(message),
                message.ProductIds,
                message.ResourceIds,
                options.Value.ProductOfferProductDetail);
        }

        var param = new WinLossWeekParam(
            message.ViewerId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            productViewModel.UserSelectedProductIds,
            productDetailsXml,
            message.CustName,
            allowedSitesToShareLoad.GetIP(message.Site));
        var weekData = await winlossService.GetWeekData(param);

        return BuildWeekViewModels(weekData, param.FromDate, param.ToDate, message.WeekFromDate, message.WeekToDate);
    }

    private static IEnumerable<WinLossWeekViewModel> BuildWeekViewModels(
        IEnumerable<WinLossWeekModel> weekData,
        DateTime fromDate,
        DateTime toDate,
        DateTime? selectedFromDate = null,
        DateTime? selectedToDate = null)
    {
        var weekDataHash = new HashSet<string>();
        foreach (var item in weekData)
        {
            weekDataHash.Add($"{item.StartOfWeek.ToShortDateWithSlash()}-{item.EndOfWeek.ToShortDateWithSlash()}");
        }

        var weekSelections = new List<WinLossWeekViewModel>();
        var startDate = fromDate.Date;
        var endDate = toDate.Date;

        while (endDate.Subtract(startDate).TotalDays >= 0)
        {
            var endDateStartOfWeek = endDate.StartOfWeek();
            var startDateOrEndDateStartOfWeek = startDate < endDateStartOfWeek ? endDateStartOfWeek : startDate;

            var startDateOrEndDateStartOfWeekString = startDateOrEndDateStartOfWeek.ToShortDateWithSlash();
            var endDateString = endDate.ToShortDateWithSlash();
            var weekSelection = new WinLossWeekViewModel(
                startDateOrEndDateStartOfWeekString,
                endDateString,
                GetDisplayWeek(startDateOrEndDateStartOfWeek, endDate),
                weekDataHash.Contains($"{startDateOrEndDateStartOfWeekString}-{endDateString}"),
                selectedFromDate.HasValue
                && selectedToDate.HasValue
                && selectedFromDate.Value == startDateOrEndDateStartOfWeek
                && selectedToDate.Value == endDate);
            weekSelections.Add(weekSelection);

            endDate = endDateStartOfWeek.AddDays(-1);
        }

        return weekSelections;
    }

    private static string GetDisplayWeek(DateTime fromDate, DateTime toDate)
    {
        var displayToDate = $"{toDate.ToString("dd")} {Resources.ResourceManager.GetString($"{toDate.ToString("MMM")}")}";

        return fromDate == toDate ? displayToDate : $"{fromDate.ToString("dd")} - {displayToDate}";
    }

    public async Task<WinLossBetListViewModel> Handle(WinLossBetListQuery message, CancellationToken cancellationToken)
    {
        var showContributionAmountRoundNote = message.IsEnabledJackpot;
        var (productViewModel, productDetailsXml) = await BuildProductViewModel(message, message.ProductIds, message.ResourceIds);
        if (options.Value.Is789Y)
        {
            productDetailsXml = await productService.BuildProductDetailsXml789y(
                await GetProducts(message),
                message.ProductIds,
                message.ResourceIds,
                options.Value.ProductOfferProductDetail);
        }

        var param = new WinLossParam(
            message.CustId,
            productViewModel.UserSelectedProductIds,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            productDetailsXml,
            message.DisplayVNIP,
            message.SuperId,
            message.Site,
            maxTotalRow: appSettings.BetListMaxTotalRowView,
            maxTotalRowConfig: appSettings.BetListMaxTotalRowView,
            ip: allowedSitesToShareLoad.GetIP(message.Site));
        var betlistData = await winlossService.GetBetList(param);
        WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

        return new WinLossBetListViewModel(
            message,
            betlistData,
            Builder,
            TicketHelper,
            showContributionAmountRoundNote);
    }
}