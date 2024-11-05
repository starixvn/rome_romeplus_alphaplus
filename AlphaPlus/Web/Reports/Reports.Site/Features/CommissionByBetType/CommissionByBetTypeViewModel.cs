using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeViewModel(
    CommissionByBetTypeQuery message,
    ReportData<CommissionByBetTypeModel> reportData) : DrillDownableViewModel<CommissionByBetTypeModel>(
        message,
        new PageHeaderViewModel(
            Title: Resources.CommissionByBettype,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: BuildBreadcrumbViewModel(message)),
        reportData,
        message.SortPropertyName,
        message.DescendingSort)
{
    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption);

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, DateRangePicker.MinDate);

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            ["CustLevelId"] = CustLevel.Value.ToString(),
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
        };
}