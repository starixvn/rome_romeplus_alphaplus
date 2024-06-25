using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewViewModel(
    WinLossDetailNewQuery message,
    ProductMultiSelectionViewModel productViewModel,
    EnteredResultSport totalEnteredResults)
{
    public UserLevel ViewerLevel { get; } = message.ViewerLevel;

    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public byte CustLevelId { get; } = message.CustLevelId;

    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: GetReportNameByLevel(message.CustLevel),
        IsFilterVisible: message.IsFilterVisible,
        TotalEnteredResults: totalEnteredResults);

    public DateRangePickerJQueryViewModel DateRangePicker { get; } = new DateRangePickerJQueryViewModel(
        message.FromDate,
        message.ToDate,
        message.MinDate,
        DateTime.Today.AddDays(1),
        message.Language);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public string Username { get; } = message.Username;

    public string VisibleColumns { get; } = message.VisibleColumns;

    public string WeekFromDate
        => message.WeekFromDate == null ? null : message.WeekFromDate.Value.ToShortDateWithSlash();

    public string WeekToDate
        => message.WeekToDate == null ? null : message.WeekToDate.Value.ToShortDateWithSlash();

    public string BreadcrumbLevel
        => string.IsNullOrWhiteSpace(message.BreadcrumbsJson)
        ? JsonHelper.ToJson(new Breadcrumb(message.ViewerId, CustName, message.ViewerLevelId))
        : message.BreadcrumbsJson;

    public bool CanDrilldown { get; } = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();

    private static string GetReportNameByLevel(UserLevel custLevel)
        => custLevel.IsSuperLevel()
        ? Resources.MasterWinLossDetail
        : custLevel.IsMasterLevel()
            ? Resources.AgentWinLossDetail
            : Resources.MemberWinLossDetail;
}
