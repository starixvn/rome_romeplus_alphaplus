namespace AlphaPlus.BetLists.CancelledBets;

public record CancelledBetParam(
    int CustId,
    DateTime FromDate,
    DateTime ToDate,
    byte RoleId,
    string ProductIds,
    string StatusIds,
    int PageIndex,
    int PageSize,
    bool DisplayVNIP,
    int SuperId,
    string System,
    string SiteName);