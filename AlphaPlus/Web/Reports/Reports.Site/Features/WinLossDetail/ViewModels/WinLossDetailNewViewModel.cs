using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewViewModel(
    WinLossDetailNewQuery message,
    ProductMultiSelectionViewModel productViewModel,
    EnteredResultSport totalEnteredResults)
{
    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: Resources.Consolidation,
        IsFilterVisible: message.IsFilterVisible,
        TotalEnteredResults: totalEnteredResults);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption);

    public bool CanDrilldown { get; } = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();
}
