using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Features.WinLossAnalysisChart;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysisChart;

public class WinLossAnalysisChartViewModel(
    WinLossAnalysisChartQuery message,
    IEnumerable<WinLossAnalysisChartModel> reportData,
    ProductMultiSelectionViewModel productViewModel)
{
    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(Resources.MonthlyWinLossChart, IsFilterVisible: message.IsFilterVisible);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public MonthPickerViewModel MonthPicker { get; }
        = new MonthPickerViewModel(message.MinDate, message.FromDate, message.ToDate);

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());

    public IEnumerable<WinLossAnalysisChartModel> ReportData { get; } = reportData;

    public DualAxisChartViewModel DualAxisChart { get; } = new DualAxisChartViewModel(
        $"{Resources.MonthlyWinLossChart} {Resources.From} {message.FromDate.ToShortDateWithSlash()} {Resources.To} {message.ToDate.ToShortDateWithSlash()}",
        reportData.Select(x => GetShortMonthNameAndShortYear(x.WinLossDate)).JoinWithComma(),
        $"{message.ViewerLevel.DisplayName} {Resources.WinLossTitle}",
        Resources.Turnover,
        reportData.Select(x => x.WinLoss).JoinWithComma(),
        reportData.Select(x => x.Turnover).JoinWithComma(),
        message.CurrencyName);

    private static string GetShortMonthNameAndShortYear(string value)
        => string.IsNullOrWhiteSpace(value)
        ? string.Empty
        : ToDate(value).ToShortMonthNameAndShortYear();

    public string ToShortMonthNameAndYear(string value)
        => string.IsNullOrWhiteSpace(value)
        ? string.Empty
        : ToDate(value).ToShortMonthNameAndYear();

    private static DateTime ToDate(string value)
    {
        var splitInts = value.ToListInt('-').ToList();

        return new DateTime(splitInts[0], splitInts[1], 1);
    }

    public bool IsDarkMode { get; } = !string.IsNullOrWhiteSpace(message.DarkModeName);
}