using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultViewModel(
    EnteredResultQuery message,
    IEnumerable<EnteredResultSport> reportData,
    EnteredResultSport totalEnteredResults)
{
    public string Title
        => message.ViewerLevel.IsSuperLevel()
        ? Resources.MasterWinLoss
        : message.ViewerLevel.IsMasterLevel() ? Resources.AgentWinLoss : Resources.MemberWinLoss;

    public DateRangePickerViewModel DateRangePickerViewModel
    {
        get
        {
            var today = DateTime.Today;
            var minDate = today.FirstDayOf3MonthsAgo();

            return new DateRangePickerViewModel(minDate, DateTime.MaxValue, today, today, 0, isSupportHistoricReport: true);
        }
    }

    public IEnumerable<EnteredResultSport> ReportData { get; } = reportData;

    public EnteredResultSport TotalEnteredResults { get; } = totalEnteredResults;
}