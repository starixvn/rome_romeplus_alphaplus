namespace AlphaPlus.Reports.Features.IPMatching;

public record IPMatchingParam(
    int CustId,
    DateTime WinlostDate,
    int MemberCount,
    bool IsAdminCanViewVNIP,
    string SiteName);
