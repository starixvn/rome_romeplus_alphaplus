using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewViewModel(
    WinLossDetailNewQuery message,
    EnteredResultSport totalEnteredResults,
    ReportData<WinLossDetailModel> reportData,
    IEnumerable<Breadcrumb> breadcrumbs,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<WinLossDetailWeekViewModel> weeks) : WinLossDetailGetDataViewModel(message, totalEnteredResults, reportData, breadcrumbs)
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
        SystemTime.Today.AddDays(1),
        message.Language);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public IEnumerable<WinLossDetailWeekViewModel> Weeks { get; } = weeks;

    public string Username { get; } = message.Username;

    public string VisibleColumns { get; } = message.VisibleColumns;

    public string WeekFromDate
        => message.WeekFromDate == null ? null : message.WeekFromDate.Value.ToShortDateWithSlash();

    public string WeekToDate
        => message.WeekToDate == null ? null : message.WeekToDate.Value.ToShortDateWithSlash();

    private static string GetReportNameByLevel(UserLevel custLevel)
        => custLevel.IsSuperLevel()
        ? Resources.MasterWinLossDetail
        : custLevel.IsMasterLevel()
            ? Resources.AgentWinLossDetail
            : Resources.MemberWinLossDetail;
}
