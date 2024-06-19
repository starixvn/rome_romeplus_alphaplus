using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.CancelledBets;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CancelledBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CancelledBets;

public class CancelledBetViewModel(
    CancelledBetQuery message,
    BetListData betListData,
    PagerViewModel pager,
    ProductMultiSelectionViewModel productViewModel,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : BetListBaseViewModel(message, betListData, builder, ticketHelper, true)
{
    public static readonly IEnumerable<int> RangeOptions =
    [
        DateRangePickerViewModel.TodayOptionValue,
        DateRangePickerViewModel.YesterdayOptionValue,
        DateRangePickerViewModel.ThisWeekOptionValue,
        DateRangePickerViewModel.LastWeekOptionValue,
        DateRangePickerViewModel.ThisMonthOptionValue,
        DateRangePickerViewModel.SpecificDateOptionValue
    ];

    public bool IsFilterVisible { get; } = message.IsFilterVisible;

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public IEnumerable<int> StatusIds { get; } = string.IsNullOrWhiteSpace(message.StatusIds)
        ? TicketStatusConstants.All
        : message.StatusIds.ToListInt();

    public IEnumerable<SelectListItem> Status =>
    [
        new SelectListItem(Resources.Void, TicketStatusConstants.Void.ToString(), StatusIds.Contains(TicketStatusConstants.Void)),
        new SelectListItem(Resources.reject, TicketStatusConstants.Reject.ToString(), StatusIds.Contains(TicketStatusConstants.Reject)),
        new SelectListItem(Resources.refund, TicketStatusConstants.Refund.ToString(), StatusIds.Contains(TicketStatusConstants.Refund))
    ];

    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption,
        true,
        RangeOptions);

    public string ViewDataInstructionMessage
        => string.Format(Resources.NoteCancelledBetPage, message.MinDate.ToShortDateWithSlash());

    public PagerViewModel Pager { get; } = pager;

    public int StartIndex { get; } = pager.PageSize * (pager.PageIndex - 1) + 1;

    protected override string GetTitle() => Resources.VoidAndReject;

    public IDictionary<string, string> GetExcelParams() => new Dictionary<string, string>
    {
        [nameof(ProductIds)] = ProductIds,
        [nameof(StatusIds)] = StatusIds.JoinWithComma(),
        [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
        [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
    };
}