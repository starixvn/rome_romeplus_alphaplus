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
        message.LevelType,
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

    protected override IDictionary<string, string> AddMoreExcelParams(IDictionary<string, string> queryParams)
    {
        queryParams[nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate;
        queryParams[nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate;

        return queryParams;
    }

    public bool IsNegative(decimal? cellValue) 
    {
        if (!cellValue.HasValue) 
        {
            return false;
        }
        var rounded = Math.Round(cellValue.Value, 2, MidpointRounding.AwayFromZero);
        return rounded < 0;
    }
}