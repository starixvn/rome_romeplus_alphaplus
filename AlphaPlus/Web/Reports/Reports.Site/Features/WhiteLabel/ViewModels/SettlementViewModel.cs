using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public record SettlementViewModel(
    SettlementQuery message,
    IEnumerable<IdNameModel> whiteLabels,
    ReportData<SettlementModel> ReportData) : SettlementGetDataViewModel(message.ViewerLevel, ReportData)
{
    public MonthPickerViewModel MonthPicker { get; } = new MonthPickerViewModel(message.MinDate, message.FromDate, message.ToDate);

    public string WhiteLabelJson { get; } = JsonHelper.ToJson(whiteLabels);

    public string Username { get; } = message.Username;
}
