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
}
