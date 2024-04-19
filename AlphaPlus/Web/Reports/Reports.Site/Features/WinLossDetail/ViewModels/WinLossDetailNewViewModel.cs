using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewViewModel(
    WinLossDetailNewQuery message,
    ProductMultiSelectionViewModel productViewModel,
    EnteredResultSport totalEnteredResults)
{
    public UserLevel ViewerLevel { get; } = message.ViewerLevel;

    public Breadcrumb BreadcrumbLevel => new Breadcrumb(message.ViewerId, message.CustName, message.ViewerLevelId);

    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: GetReportNameByLevel(message.CustLevel),
        IsFilterVisible: message.IsFilterVisible,
        TotalEnteredResults: totalEnteredResults);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerJQueryViewModel DateRangePicker { get; } = new DateRangePickerJQueryViewModel(
        message.FromDate,
        message.ToDate,
        message.MinDate,
        DateTime.Today.AddDays(1),
        message.Language);

    public bool CanDrilldown { get; } = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();

    private static string GetReportNameByLevel(UserLevel custLevel)
        => custLevel.IsSuperLevel()
        ? Resources.MasterWinLossDetail
        : custLevel.IsMasterLevel()
            ? Resources.AgentWinLossDetail
            : Resources.MemberWinLossDetail;
}
