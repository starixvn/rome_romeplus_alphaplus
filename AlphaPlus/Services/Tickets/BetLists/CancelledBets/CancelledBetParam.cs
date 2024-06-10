namespace AlphaPlus.BetLists.CancelledBets;

public record CancelledBetParam(
    int CustId,
    DateTime FromDate,
    DateTime ToDate,
    DateTime MoveBetsDay,
    byte RoleId,
    string ProductIds,
    string StatusIds,
    int PageIndex,
    int PageSize,
    bool DisplayVNIP,
    string SiteName);