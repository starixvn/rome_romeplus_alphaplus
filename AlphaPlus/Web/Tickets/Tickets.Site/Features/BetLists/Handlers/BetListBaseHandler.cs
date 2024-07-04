using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers;

public class BetListBaseHandler
{
    protected BetListBaseHandler(
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper,
        ISmartPunterBetListMonitor monitorService,
        IPService ipService,
        IOptions<AppSettings> options)
    {
        Builder = builder;
        TicketHelper = ticketHelper;
        MonitorService = monitorService;
        TicketHelper.IsVnIp = ipService.IsVNIP;
        TicketHelper.HideSabaText = options.Value.IsSiteLabel777;
    }

    protected internal IBetListHTMLBuilder Builder { get; }

    protected internal ITicketHelper TicketHelper { get; }

    protected internal ISmartPunterBetListMonitor MonitorService { get; }

    protected internal void WriteLog(int custId, IEnumerable<TicketDto> tickets)
         => MonitorService.WriteLog(custId, tickets.ToList<ITicket>());

    protected static ProductMultiSelectionViewModel BuildProductViewModel(
        IEnumerable<Product> products,
        string userSelectedProductIds,
        string language)
    {
        var (productGroups, validUserSelectedProductIds) = BuildProductGroups(products, userSelectedProductIds);

        return new ProductMultiSelectionViewModel(productGroups, validUserSelectedProductIds.JoinWithComma(), language);
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

    // protected internal IEnumerable<GroupProductViewModel> BuildProductSelectListItems(
    //     IEnumerable<int> userSelectedProductIds,
    //     IEnumerable<Product> products)
    //     => products?.Any() == true
    //         ? products
    //             .GroupBy(product => new { product.Type, product.TypeName })
    //             .Select(grp => new GroupProductViewModel(
    //                 new SelectListGroup { Name = grp.Key.TypeName },
    //                 grp.Select(product => new SelectListItem
    //                 {
    //                     Text = product.DisplayName,
    //                     Value = product.Id.ToString(),
    //                     Selected = userSelectedProductIds.Contains(product.Id)
    //                 })))
    //         : Enumerable.Empty<GroupProductViewModel>();

    // protected internal static IEnumerable<ProductViewModel> BuildProductViewModels(
    //     IEnumerable<Product> products,
    //     WinLossIndexQuery message)
    // {
    //     var productList = new List<ProductViewModel>();

    //     if (products?.Any() == true)
    //     {
    //         var userSelectedProductIds = message.UserSelectedProductIds;
    //         products
    //             .GroupBy(product => new { product.Type, product.TypeName })
    //             .ToList()
    //             .ForEach(item =>
    //             {
    //                 if (item.Key.Type == 0)
    //                 {
    //                     productList.AddRange(item.Select(p => new ProductViewModel(
    //                         p.Id,
    //                         p.DisplayName,
    //                         userSelectedProductIds.ContainsKey(p.Id),
    //                         p.EndOfSupportDate.HasValue && p.EndOfSupportDate < message.HistoricPivotDate,
    //                         null)));
    //                 }
    //                 else
    //                 {
    //                     var children = item.Select(p => new ProductViewModel(
    //                         p.Id,
    //                         p.DisplayName,
    //                         userSelectedProductIds.ContainsKey(p.Id),
    //                         p.EndOfSupportDate.HasValue && p.EndOfSupportDate < message.HistoricPivotDate,
    //                         null)).ToList();
    //                     var isGroupProductChecked = message.FromDate >= message.HistoricPivotDate
    //                         ? children.Where(p => !p.OnlyShowHistoric).All(p => p.Checked)
    //                         : children.All(p => p.Checked);
    //                     productList.Add(new ProductViewModel(
    //                         item.Key.Type,
    //                         item.Key.TypeName,
    //                         isGroupProductChecked,
    //                         false,
    //                         children));
    //                 }
    //             });
    //     }

    //     return productList;
    // }

    // protected internal static IEnumerable<ProductDetailViewModel> BuildProductDetailViewModels(
    //     IEnumerable<ProductDetail> productDetails,
    //     IDictionary<int, bool> userSelectedResourceIds)
    //     => productDetails.Any()
    //     ? productDetails.Select(p => new ProductDetailViewModel(
    //         p.ProductId,
    //         p.ResourceId,
    //         p.DisplayName,
    //         userSelectedResourceIds.ContainsKey(p.ResourceId)))
    //     : Enumerable.Empty<ProductDetailViewModel>();


    // protected internal static IEnumerable<WeekSelectionViewModel> BuildWeekSelectionViewModels(
    //     IEnumerable<TicketDto> tickets,
    //     DateTime fromDate,
    //     DateTime toDate,
    //     DateTime? selectedFromDate,
    //     DateTime? selectedToDate)
    // {
    //     var weekSelections = BuildWeekSelectionViewModels(fromDate, toDate, selectedFromDate, selectedToDate);

    //     foreach (var item in weekSelections)
    //     {
    //         item.IsEnable = tickets.Any(t => item.FromDate <= t.WinlostDate && t.WinlostDate <= item.ToDate);
    //     }

    //     return weekSelections;
    // }

    // protected internal static IEnumerable<WeekSelectionViewModel> BuildWeekSelectionViewModels(
    //     DateTime fromDate,
    //     DateTime toDate,
    //     DateTime? selectedFromDate,
    //     DateTime? selectedToDate)
    // {
    //     var weekSelections = new List<WeekSelectionViewModel>();
    //     var startDate = fromDate.Date;
    //     var endDate = toDate.Date;

    //     while (endDate.Subtract(startDate).TotalDays >= 0)
    //     {
    //         var endDateStartOfWeek = endDate.StartOfWeek();
    //         var startDateOrEndDateStartOfWeek = startDate < endDateStartOfWeek ? endDateStartOfWeek : startDate;
    //         weekSelections.Add(
    //             new WeekSelectionViewModel(startDateOrEndDateStartOfWeek, endDate, selectedFromDate, selectedToDate));

    //         endDate = endDateStartOfWeek.AddDays(-1);
    //     }

    //     return weekSelections;
    // }
}
