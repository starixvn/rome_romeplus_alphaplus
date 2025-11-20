namespace AlphaPlus.BetLists.RunningBetList;

public record RunningBetListParam(
    int CustId,
    string ProductIds,
    DateTime FromDate,
    DateTime ToDate,
    bool GetAll,
    string UserName,
    bool DisplayVNIP,
    int SuperId,
    string System,
    string SiteName);
