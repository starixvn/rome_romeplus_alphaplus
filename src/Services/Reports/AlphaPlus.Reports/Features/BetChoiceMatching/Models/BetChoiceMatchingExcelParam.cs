namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public record BetChoiceMatchingExcelParam(
    int CustId,
    DateTime WinLossDate,
    int BetCount,
    bool IsAdminCanViewVNIP,
    string SiteName,
    string Username,
    decimal? Stake,
    MemberWinLossOperators MemberWinLossOperator,
    decimal? MemberWinLossValue
) : BetChoiceMatchingParam(CustId, WinLossDate, BetCount, IsAdminCanViewVNIP, SiteName, true);