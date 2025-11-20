namespace AlphaPlus.BetLists.LastBets;

public record GetLastBetsParam(
    int BetType,
    int CustId,
    long TransId,
    int PageSize,
    int RunningMatchCacheTime,
    bool DisplayVNIP,
    int SuperId,
    string System,
    string SiteName);
