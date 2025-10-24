using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.WinLossByProduct;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossByProduct;

public class WinLossByProductViewModel(
    WinLossByProductQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<WinLossByProductModel> reportData,
    WinLossByBetTypeModel total)
{
    public UserLevel CustLevel { get; } = message.ViewerLevel;

    public string CustLevelName { get; } = SharedResources.ResourceManager.GetString(message.ViewerLevel.DisplayName);

    public bool IsSetDefaultStackMode { get; } = message.IsSetDefaultStackMode;

    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: Resources.WinLossByProduct,
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

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, DateRangePicker.MinDate);

    public IEnumerable<WinLossByProductModel> ReportData { get; } = reportData;

    public WinLossByBetTypeModel Total { get; } = total;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds,
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
        };
    
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