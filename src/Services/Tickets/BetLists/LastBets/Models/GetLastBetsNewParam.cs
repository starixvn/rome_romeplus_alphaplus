namespace AlphaPlus.BetLists.LastBets;

public record GetLastBetsNewParam(
    int CustId,
    long TransId,
    string MemberList,
    string SportTypeList,
    string LeagueList,
    string BetTypeList,
    byte LiveIndicator,
    decimal MinStake,
    int LimitRowCount,
    int RunningMatchCacheTime,
    bool DisplayVNIP,
    int SuperId,
    string SiteName,
    byte UserLevel);