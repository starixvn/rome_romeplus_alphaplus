using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RunningBetList;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RunningBetList;

public class RunningBetListViewModel(
    RunningBetListQuery message,
    BetListData betListData,
    ProductMultiSelectionViewModel productViewModel,
    HashSet<int> supportScoreMapBetTypeIds,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : BetListBaseViewModel(message, betListData, builder, ticketHelper, supportRiskAmount: true)
{
    public bool GetAll { get; } = message.GetAll;

    public bool AllowToViewSuperPT { get; } = message.ViewerLevel.IsSuperLevel();

    public bool AllowToViewMasterPT { get; } = message.ViewerLevel.IsMasterOrSuperLevel();

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());

    public bool IsFilterVisible { get; } = message.IsFilterVisible;

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption,
        true,
        [
            DateRangePickerViewModel.TodayOptionValue,
            DateRangePickerViewModel.YesterdayOptionValue,
            DateRangePickerViewModel.SpecificDateOptionValue
        ]);

    public override bool CanViewScoreMap(ITicket ticket)
        => supportScoreMapBetTypeIds.Contains(ticket.BetTypeId);

    protected override string GetTitle() => $"{Resources.BetlistRunning} - {message.CustName}";

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

    public string GetMemberBetListUrl()
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(CustId), CustId.ToString()),
            KeyValuePair.Create(nameof(CustName), CustName),
            KeyValuePair.Create("FromDate", SystemTime.Today.AddDays(-1).ToShortDateWithSlash()),
            KeyValuePair.Create("ToDate", SystemTime.Today.ToShortDateWithSlash()),
            KeyValuePair.Create(nameof(ProductIds), ProductIds),
            KeyValuePair.Create("UserSelectedDateRangeOption", DateRangePickerViewModel.SpecificDateOptionValue.ToString())
        });

        return MicroSites.TicketUrl($"~/MemberBetList/Index{queryString}");
    }
}
