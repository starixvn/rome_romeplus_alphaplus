using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewViewModel(
    WinLossDetailNewQuery message,
    ProductMultiSelectionViewModel productViewModel,
    EnteredResultSport totalEnteredResults)
{
    public UserLevel ViewerLevel { get; } = message.ViewerLevel;

    public string BreadcrumbLevel
        => string.IsNullOrWhiteSpace(message.BreadcrumbsJson)
        ? JsonHelper.ToJson(new Breadcrumb(message.ViewerId, message.CustName, message.ViewerLevelId))
        : message.BreadcrumbsJson;

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

    public string Username { get; } = message.Username;

    public string WeekFromDate
        => message.WeekFromDate == null ? null : message.WeekFromDate.Value.ToShortDateWithSlash();

    public string WeekToDate
        => message.WeekToDate == null ? null : message.WeekToDate.Value.ToShortDateWithSlash();

    public bool CanDrilldown { get; } = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();

    private static string GetReportNameByLevel(UserLevel custLevel)
        => custLevel.IsSuperLevel()
        ? Resources.MasterWinLossDetail
        : custLevel.IsMasterLevel()
            ? Resources.AgentWinLossDetail
            : Resources.MemberWinLossDetail;
}
