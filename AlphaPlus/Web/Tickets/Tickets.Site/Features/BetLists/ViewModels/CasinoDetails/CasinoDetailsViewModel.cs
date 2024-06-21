using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CasinoDetails;

public class CasinoDetailsViewModel(
    CasinoDetailsQuery message,
    BetListData betListData,
    PagerViewModel pager,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper)
        : WinLossViewModel(message, betListData, builder, ticketHelper, false, startIndex: pager.PageSize * (pager.PageIndex - 1) + 1)
{
    public PagerViewModel Pager { get; } = pager;

    protected override string GetTitle()
        => $"{(!string.IsNullOrWhiteSpace(message.SportName) ? message.SportName : "RNG Casino")} Details";

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(message.RefNo)] = message.RefNo.ToString(),
            [nameof(message.TransType)] = message.TransType.ToString(),
            [nameof(message.SportName)] = message.SportName,
            [nameof(message.ProductId)] = message.ProductId.ToString(),
            [nameof(CustId)] = CustId.ToString()
        };
}
