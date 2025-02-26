using AlphaPlus.Shared.AppResources.Betlist.Site;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;

public class WinLossNewViewModel(
    WinLossNewQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<WinLossWeekViewModel> weeks,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    bool showContributionAmountRoundNote,
    bool isGroupHeadline = false,
    bool showMemberName = false,
    string[] excludingTicketStatus = null)
        : WinLossWeekSelectionViewModel(message, betListData, builder, ticketHelper, showContributionAmountRoundNote, isGroupHeadline, showMemberName, excludingTicketStatus)
{
    public DateRangePickerJQueryViewModel DateRangePicker { get; } = new DateRangePickerJQueryViewModel(
        message.FromDate,
        message.ToDate,
        message.MinDate,
        SystemTime.Today.AddDays(1),
        message.Language);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public string VisibleColumns { get; } = message.VisibleColumns;

    public IEnumerable<WinLossWeekViewModel> Weeks { get; } = weeks;

    protected override string GetTitle() => $"{Resources.memberBetList} - {CustName}";
}