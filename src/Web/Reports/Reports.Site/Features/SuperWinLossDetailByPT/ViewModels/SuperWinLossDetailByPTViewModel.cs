using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTViewModel(
    SuperWinLossDetailByPTQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<SuperWinLossDetailByPTModel> reportData,
    SuperWinLossDetailByPTEntity total,
    IEnumerable<SuperPTProductViewModel> superPTProductViewModels,
    bool isMobileView)
{
    public bool IsMobileView { get; } = isMobileView;

    public string SortPropertyName { get; } = message.SortPropertyName;

    public byte DescendingSort { get; } = message.DescendingSort;

    public bool IsSetDefaultStackMode { get; } = message.IsSetDefaultStackMode;

    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: Resources.SuperWinLossDetailByPT,
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

    public IEnumerable<SuperWinLossDetailByPTModel> ReportData { get; } = reportData;

    public SuperWinLossDetailByPTEntity Total { get; } = total;

    public IEnumerable<SuperPTProductViewModel> SuperPTProductViewModels { get; } = superPTProductViewModels;

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