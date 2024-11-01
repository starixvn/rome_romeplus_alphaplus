using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionViewModel(
    ProgressiveContributionQuery message,
    IEnumerable<ProgressiveContributionModel> reportData,
    ProgressiveContributionEntity total,
    ProductMultiSelectionViewModel productViewModel)
{
    public int CustId { get; } = message.ViewerId;

    public UserLevel CustLevel { get; } = message.ViewerLevel;

    public bool IsHistoricReport { get; } = message.IsHistoricReport;

    public bool IsSetDefaultStackMode { get; } = message.IsSetDefaultStackMode;

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(
            Resources.ProgressiveGamesContribution,
            HasStackIcon: message.ViewerLevel.IsMasterOrSuperLevel(),
            HasHideFilterIcon: true,
            IsStackMode: message.IsStackMode,
            IsFilterVisible: message.IsFilterVisible);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption,
            isSupportHistoricReport: true,
            isHistoricReport: message.IsHistoricReport);

    public MonthPickerViewModel MonthPicker { get; }
        = new MonthPickerViewModel(message.MinDate, message.FromDate, message.ToDate);

    public string ViewDataInstructionMessage
        => string.Format(
            IsHistoricReport
                ? Resources.CanOnlyViewDataAfterTimeHistoryReport
                : Resources.CanOnlyViewDataAfterTimeCurrentReport,
            message.MinDate.ToShortDateWithSlash());

    public string ViewActionMessage => IsHistoricReport ? Resources.BackToCurrentReport : string.Empty;

    public IEnumerable<ProgressiveContributionModel> ReportData { get; } = reportData;

    public ProgressiveContributionEntity Total { get; } = total;

    public int ContributionShareColumnsCount => message.ViewerLevelId - 1;

    public int ReportColumnsCount
        => message.ViewerLevel.IsSuperLevel() ? 8 : message.ViewerLevel.IsMasterLevel() ? 7 : 6;

    public IDictionary<string, string> GetExcelParams() => new Dictionary<string, string>
    {
        [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
        [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate,
        [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
        [nameof(IsHistoricReport)] = IsHistoricReport.ToString()
    };
}