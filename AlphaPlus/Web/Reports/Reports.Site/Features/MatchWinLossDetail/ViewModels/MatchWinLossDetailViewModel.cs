using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailViewModel(
    MatchWinLossDetailQuery message,
    ProductMultiSelectionViewModel productViewModel,
    LeagueViewModel leagueViewModel,
    MatchWinLossDetailReport reportData)
{
    public UserLevel CustLevel { get; } = message.ViewerLevel;

    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Resources.MatchWinLossDetail,
        HasStackIcon: true,
        IsStackMode: message.IsStackMode,
        IsFilterVisible: message.IsFilterVisible);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption);

    public LeagueViewModel LeagueViewModel { get; } = leagueViewModel;

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, DateRangePicker.MinDate);

    public MatchWinLossDetailReport ReportData { get; } = reportData;

    public bool IsSetDefaultStackMode { get; } = message.IsSetDefaultStackMode;

    public int ReportColumnsCount { get; }
        = message.ViewerLevel.IsSuperLevel()
        ? 17
        : message.ViewerLevel.IsMasterLevel() ? 14 : 11;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate,
            [nameof(LeagueViewModel.UserSelectedLeagueIds)] = LeagueViewModel.UserSelectedLeagueIds.JoinWithComma(),
            [nameof(LeagueViewModel.IsUserSelectsAllLeagues)] = LeagueViewModel.IsUserSelectsAllLeagues.ToString()
        };

    public string GetBetListUrl(MatchWinLossDetailModel match, BetTypeWinLossModel betType)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create("CustId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.CustName), message.CustName),
            KeyValuePair.Create("CustLevelId", message.ViewerLevelId.ToString()),
            KeyValuePair.Create("BetType", betType.Id.ToString()),
            KeyValuePair.Create("LiveIndicator", betType.IsLive ? "1" : "0"),
            KeyValuePair.Create(nameof(match.LeagueId), match.LeagueId.ToString()),
            KeyValuePair.Create(nameof(DateRangePicker.FromDate), DateRangePicker.FromDate),
            KeyValuePair.Create(nameof(DateRangePicker.ToDate), DateRangePicker.ToDate),
            match.Race > 0
                ? KeyValuePair.Create(nameof(betType.Race), betType.Race.ToString())
                : KeyValuePair.Create(nameof(match.MatchId), match.MatchId.ToString())
        ]);

        return $"{WinLossByMatchBetListPath}{queryString}";
    }

    public string GetParlayBetListUrl(int betType)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create("CustId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.CustName), message.CustName),
            KeyValuePair.Create("CustLevelId", message.ViewerLevelId.ToString()),
            KeyValuePair.Create(nameof(DateRangePicker.FromDate), DateRangePicker.FromDate),
            KeyValuePair.Create(nameof(DateRangePicker.ToDate), DateRangePicker.ToDate),
            KeyValuePair.Create(nameof(betType), betType.ToString()),
        ]);

        return $"{ParlayWinLossByMatchBetListPath}{queryString}";
    }

    public string GetBetListUrlByBetType(int betType)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create("CustId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.CustName), message.CustName),
            KeyValuePair.Create("CustLevelId", message.ViewerLevelId.ToString()),
            KeyValuePair.Create(nameof(DateRangePicker.FromDate), DateRangePicker.FromDate),
            KeyValuePair.Create(nameof(DateRangePicker.ToDate), DateRangePicker.ToDate),
            KeyValuePair.Create(nameof(betType), betType.ToString()),
        ]);

        return $"{WinLossByMatchBetListPath}{queryString}";
    }

    private static string WinLossByMatchBetListPath = MicroSites.TicketUrl("~/WinLossByMatch/Index");

    private static string ParlayWinLossByMatchBetListPath = MicroSites.TicketUrl("~/ParlayWinLossByMatch/Index");
}
