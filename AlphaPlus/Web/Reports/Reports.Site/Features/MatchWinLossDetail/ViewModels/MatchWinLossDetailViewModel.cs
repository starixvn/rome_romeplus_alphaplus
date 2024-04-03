using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

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
}
