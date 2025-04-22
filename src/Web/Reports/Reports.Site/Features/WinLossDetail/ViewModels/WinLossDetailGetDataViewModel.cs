using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailGetDataViewModel(
    WinLossDetailGetDataQuery message,
    EnteredResultSport totalEnteredResults,
    ReportData<WinLossDetailModel> reportData,
    IEnumerable<Reports.Features.Shared.Models.Breadcrumb> breadcrumbs)
{
    public UserLevel CustLevel { get; } = message.CustLevel;

    public byte DownlineLevelId { get; } = (byte)(message.CustLevelId - 1);

    public bool CanDrilldown { get; } = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();

    public bool CanShowMonitoredIcon { get; } = !message.IsProtectedMode && message.ViewerLevel.IsAgentLevel();

    public bool CanUpdateAccount { get; } = message.CanUpdateAccount;

    public EnteredResultSport TotalEnteredResults { get; } = totalEnteredResults;

    public ReportData<WinLossDetailModel> ReportData { get; } = reportData;

    public IEnumerable<Reports.Features.Shared.Models.Breadcrumb> Breadcrumbs { get; } = breadcrumbs;
}
