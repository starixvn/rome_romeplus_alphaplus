namespace AlphaPlus.BetLists.MemberBetList;

public record MemberBetListParam(
    int CustId,
    string ProductIds,
    DateTime FromDate,
    DateTime ToDate,
    string UserName,
    bool DisplayVNIP,
    int SuperId,
    string SiteName);
