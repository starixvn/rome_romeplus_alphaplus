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
    ITicketHelper ticketHelper,
    string exportExcelURl = "Excel")
        : WinLossViewModel(message, betListData, builder, ticketHelper, false, startIndex: pager.PageSize * (pager.PageIndex - 1) + 1)
{
    public long RefNo { get; } = message.RefNo;

    public byte TransType { get; } = message.TransType;

    public string SportName { get; } = message.SportName;

    public int ProductId { get; } = message.ProductId;

    public PagerViewModel Pager { get; } = pager;

    public string ExportExcelUrl { get; } = exportExcelURl;

    protected override string GetTitle()
        => $"{(!string.IsNullOrWhiteSpace(message.SportName) ? message.SportName : "RNG Casino")} Details";

    public override IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(RefNo)] = RefNo.ToString(),
            [nameof(TransType)] = TransType.ToString(),
            [nameof(SportName)] = SportName,
            [nameof(ProductId)] = ProductId.ToString(),
            [nameof(CustId)] = CustId.ToString()
        };
}
