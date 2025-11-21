
namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public record BetChoiceMatchingParam(
    int CustId,
    DateTime WinLossDate,
    int BetCount,
    bool IsAdminCanViewVNIP,
    string SiteName,
    int SuperId,
    string System,
    bool IsExportExcel = false);
