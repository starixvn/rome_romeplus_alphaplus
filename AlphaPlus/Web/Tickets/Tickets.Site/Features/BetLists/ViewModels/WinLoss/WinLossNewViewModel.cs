using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;

public class WinLossNewViewModel(
    WinLossNewQuery message,
    IEnumerable<ProductMultiSelectionViewModel> productViewModel,
    IEnumerable<WinLossWeekViewModel> weeks,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    bool showContributionAmountRoundNote,
    bool isGroupHeadline = false,
    bool showMemberName = false,
    string[] excludingTicketStatus = null)
        : WinLossWeekSelectionViewModel(message, weeks, betListData, builder, ticketHelper, showContributionAmountRoundNote, isGroupHeadline, showMemberName, excludingTicketStatus)
{
    public DateRangePickerJQueryViewModel DateRangePicker { get; } = new DateRangePickerJQueryViewModel(
        message.FromDate,
        message.ToDate,
        message.MinDate,
        SystemTime.Today.AddDays(1),
        message.Language);

    public IEnumerable<ProductMultiSelectionViewModel> ProductViewModel { get; } = productViewModel;

    public string ShowColumns { get; } = message.ShowColumns;
}