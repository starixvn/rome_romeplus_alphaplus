using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public record DailyWinLossViewModel(
    DailyWinLossQuery message,
    IEnumerable<IdNameModel> whiteLabels,
    ReportData<DailyWinLossModel> ReportData) : DailyWinLossGetDataViewModel(message.ViewerLevel, ReportData)
{
    public DateRangePickerJQueryViewModel DateRangePicker { get; } = new DateRangePickerJQueryViewModel(
        message.FromDate,
        message.ToDate,
        message.MinDate,
        SystemTime.Today,
        message.Language);

    public string WhiteLabelJson { get; } = JsonHelper.ToJson(whiteLabels);

    public string Username { get; } = message.Username;
}