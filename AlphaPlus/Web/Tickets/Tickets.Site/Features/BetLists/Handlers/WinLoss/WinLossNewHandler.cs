using System.Text;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.BetLists.WinLoss;
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

    public async Task<WinLossNewViewModel> Handle(WinLossNewQuery message, CancellationToken cancellationToken)
    {
        var showContributionAmountRoundNote = message.IsEnabledJackpot;
        var (productViewModel, _) = await BuildProductViewModel(message, message.ProductIds, message.ResourceIds);

        throw new NotImplementedException();
    }

    protected async Task<(ProductMultiSelectionViewModel, string)> BuildProductViewModel(
        WinLossWeekSelectionQuery message,
        string userSelectedProductIds,
        string userSelectedResourceIds)
    {
        var products = GetProducts(message);
        var (productGroups, validUserSelectedProductIds) = BuildProductViewModel(products, userSelectedProductIds);
        var (productDetails, validUserSelectedResourceIds, unselectedProductIds, productDetailXml) = await BuildProductDetailViewModel(
            message.ViewerId,
            products,
            validUserSelectedProductIds,
            userSelectedResourceIds);
        if (unselectedProductIds.Any())
        {
            validUserSelectedProductIds = validUserSelectedProductIds.Except(unselectedProductIds);
        }

        return (new ProductMultiSelectionViewModel(
            productGroups,
            validUserSelectedProductIds.JoinWithComma(),
            message.Language,
            productDetails,
            validUserSelectedResourceIds.JoinWithComma()), productDetailXml);
    }

    private static (IEnumerable<ProductGroupDTO>, IEnumerable<int>) BuildProductViewModel(
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

    private async Task<(IEnumerable<ProductDetailDTO>, IEnumerable<int>, IEnumerable<int>, string)> BuildProductDetailViewModel(
        int custId,
        IEnumerable<Product> products,
        IEnumerable<int> validUserSelectedProductIds,
        string userSelectedResourceIds)
    {
        var isCheckedAll = "*" == userSelectedResourceIds;
        var userSelectedResourceIdsList = isCheckedAll ? [] : userSelectedResourceIds.ToListInt().ToList();

        var productDetails = new List<ProductDetailDTO>();
        var validUserSelectedResourceIds = new List<int>();
        var allProductsSupportProductDetails = new List<int>();
        var selectedProductIds = new List<int>();

        var xml = new StringBuilder();
        var allProductDetails = await GetProductDetails(custId, products);
        foreach (var item in allProductDetails)
        {
            var isChecked = isCheckedAll || (validUserSelectedProductIds.Contains(item.ProductId) && userSelectedResourceIdsList.Contains(item.ResourceId));
            if (isChecked)
            {
                validUserSelectedResourceIds.Add(item.ResourceId);
                selectedProductIds.Add(item.ProductId);
                xml.Append(productService.BuildProductDetailXml(item));
            }

            productDetails.Add(new ProductDetailDTO(item.ProductId, item.ResourceId, item.DisplayName, isChecked));
            allProductsSupportProductDetails.Add(item.ProductId);
        }

        // Get un-selected Product Ids
        var unselectedProductIds = allProductsSupportProductDetails.Except(selectedProductIds);

        return (productDetails, validUserSelectedResourceIds, unselectedProductIds.ToList(), $"<ROOT>{xml}</ROOT>");
    }

    private IEnumerable<Product> GetProducts(WinLossQuery message)
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
}