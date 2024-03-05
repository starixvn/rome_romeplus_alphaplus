using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionViewModel(
    ProgressiveContributionQuery message,
    IEnumerable<ProgressiveContributionModel> reportData,
    ProgressiveContributionEntity total,
    ProductMultiSelectionViewModel productViewModel)
{
    public UserLevel ViewerLevel => message.ViewerLevel;

    public string Title => Resources.ProgressiveGamesContribution;

    public bool IsFilterVisible => message.IsFilterVisible;

    public bool IsStackMode => message.IsStackMode;

    public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

    public bool IsHistoricReport => message.IsHistoricReport;

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerViewModel DateRangePickerViewModel
        => new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption,
            isSupportHistoricReport: true,
            isHistoricReport: message.IsHistoricReport);

    public MonthPickerViewModel MonthPickerViewModel
        => new MonthPickerViewModel(message.MinDate, message.FromDate, message.ToDate);

    public string ViewDataInstructionMessage
        => string.Format(
            message.IsHistoricReport
                ? Resources.CanOnlyViewDataAfterTimeHistoryReport
                : Resources.CanOnlyViewDataAfterTimeCurrentReport,
            message.MinDate.ToShortDateWithSlash());

    public string ViewActionMessage => IsHistoricReport ? Resources.BackToCurrentReport : string.Empty;

    public IEnumerable<ProgressiveContributionModel> ReportData { get; } = reportData;

    public ProgressiveContributionEntity Total { get; } = total;

    public int ContributionShareColumnsCount => message.ViewerLevelId - 1;

    public int ReportColumnsCount
        => message.ViewerLevel.IsSuperLevel() ? 8 : message.ViewerLevel.IsMasterLevel() ? 7 : 6;
}