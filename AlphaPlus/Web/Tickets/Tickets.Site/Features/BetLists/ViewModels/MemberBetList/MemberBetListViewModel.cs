using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.MemberBetList;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.MemberBetList;

public class MemberBetListViewModel(
    MemberBetListQuery message,
    BetListData betListData,
    ProductMultiSelectionViewModel productViewModel,
    HashSet<int> supportScoreMapBetTypeIds,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper)
        : WinLossViewModel(message, betListData, builder, ticketHelper, message.IsEnabledJackpot, false, false, ExcludingTicketStatus, supportRiskAmount: true)
{
    private static readonly string[] ExcludingTicketStatus =
    [
        BetStatus.Running,
        BetStatus.Waiting,
        BetStatus.Void,
        BetStatus.Reject,
        BetStatus.Pending
    ];

    private static readonly IEnumerable<int> RangeOptions =
    [
        DateRangePickerViewModel.TodayOptionValue,
        DateRangePickerViewModel.YesterdayOptionValue,
        DateRangePickerViewModel.SpecificDateOptionValue
    ];

    public bool IsFilterVisible => message.IsFilterVisible;

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption,
        true,
        RangeOptions);

    public override bool CanViewScoreMap(ITicket ticket)
        => ticket.Status.Equals(BetStatus.Running, StringComparison.OrdinalIgnoreCase)
        && supportScoreMapBetTypeIds.Contains(ticket.BetTypeId);

    protected override string GetTitle() => $"{Resources.BetListHistory} - {message.CustName}";

    public string GetDailyStatementUrl()
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(CustId), CustId.ToString()),
            KeyValuePair.Create(nameof(CustName), CustName),
            KeyValuePair.Create("CustLevelId", "1"),
            KeyValuePair.Create(nameof(ProductIds), ProductIds)
        });

        return MicroSites.ReportsUrl($"~/Statement/MemberStatement{queryString}");
    }

    public string GetRunningBetListUrl()
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(CustId), CustId.ToString()),
            KeyValuePair.Create(nameof(CustName), CustName),
            KeyValuePair.Create("FromDate", SystemTime.Today.AddDays(-7).ToShortDateWithSlash()),
            KeyValuePair.Create("ToDate", SystemTime.Today.ToShortDateWithSlash()),
            KeyValuePair.Create(nameof(ProductIds), ProductIds)
        });

        return MicroSites.TicketUrl($"~/RunningBetList/Index{queryString}");
    }
}