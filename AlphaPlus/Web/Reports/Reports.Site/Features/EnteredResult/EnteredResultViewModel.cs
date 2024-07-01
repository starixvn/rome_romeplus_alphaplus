using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultViewModel(EnteredResultQuery message, ReportData<EnteredResultSport> reportData)
{
    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        message.ViewerLevel.IsSuperLevel()
            ? Resources.MasterWinLoss
            : message.ViewerLevel.IsMasterLevel() ? Resources.AgentWinLoss : Resources.MemberWinLoss,
        IsFilterVisible: message.IsFilterVisible,
        TotalEnteredResults: reportData.Total);

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            SystemTime.Today,
            SystemTime.Today,
            DateRangePickerViewModel.TodayOptionValue,
            isSupportHistoricReport: true);

    public ReportData<EnteredResultSport> ReportData { get; } = reportData;
}