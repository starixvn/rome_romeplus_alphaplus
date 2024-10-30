using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public class WinLossAnalysisViewModel(
    WinLossAnalysisQuery message,
    ReportData<WinLossAnalysisModel> reportData,
    ProductMultiSelectionViewModel productViewModel,
    string reportTitle,
    string reportTypeName)
{
    public UserLevel CustLevel { get; } = message.ViewerLevel;

    public string SortPropertyName { get; } = message.SortPropertyName;

    public byte DescendingSort { get; } = message.DescendingSort;

    public bool IsSetDefaultStackMode { get; } = message.IsSetDefaultStackMode;

    public int ReportTypeId { get; } = message.ReportTypeId;

    public string ReportTypeName { get; } = reportTypeName;

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(
            reportTitle,
            HasStackIcon: true,
            IsStackMode: message.IsStackMode,
            IsFilterVisible: message.IsFilterVisible);

    public ReportData<WinLossAnalysisModel> ReportData { get; } = reportData;

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public IEnumerable<SelectListItem> ReportTypes { get; }
        = [
            new SelectListItem(
                Resources.Daily,
                WinLossAnalysisConstants.Daily.ToString(),
                WinLossAnalysisConstants.Daily == message.ReportTypeId),
            new SelectListItem(
                Resources.Monthly,
                WinLossAnalysisConstants.Monthly.ToString(),
                WinLossAnalysisConstants.Monthly == message.ReportTypeId),
            new SelectListItem(
                Resources.Product,
                WinLossAnalysisConstants.Product.ToString(),
                WinLossAnalysisConstants.Product == message.ReportTypeId)
        ];

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption);

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(ReportTypeId)] = ReportTypeId.ToString(),
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
        };
}