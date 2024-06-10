using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CasinoDetails;

public class CasinoDetailsViewModel(
    CasinoDetailsQuery message,
    PagedReportData<BetListData> reportData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper)
        : BetListBaseViewModel(message, reportData.DataSource.FirstOrDefault(), builder, ticketHelper)
{
    public long RefNo { get; } = message.RefNo;

    public byte TransType { get; } = message.TransType;

    public int ProductId { get; } = message.ProductId;

    public PagerViewModel Pager { get; } = new PagerViewModel(reportData.PageIndex, reportData.PageSize, reportData.TotalRecords);

    public int StartNo { get; } = reportData.PageSize * (reportData.PageIndex - 1) + 1;

    protected override string GetTitle()
        => $"{(!string.IsNullOrEmpty(message.SportName) ? message.SportName : "RNG Casino")} Details";
}
