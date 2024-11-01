using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeViewModel(
    CommissionByBetTypeQuery message,
    ReportData<CommissionByBetTypeModel> reportData,
    BreadcrumbViewModel breadcrumbLevel)
{
    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public UserLevel CustLevel { get; } = message.CustLevel;

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(
            Resources.CommissionByBettype,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbLevel);

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption);

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, DateRangePicker.MinDate);

    public byte DescendingSort { get; } = message.DescendingSort;

    public string SortPropertyName { get; } = message.SortPropertyName;

    public ReportData<CommissionByBetTypeModel> ReportData { get; } = reportData;

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