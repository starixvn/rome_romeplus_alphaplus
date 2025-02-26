namespace AlphaPlus.BetLists.WinLossByMatch;

public record WinLossByMatchParam(
    int CustId,
    int LeagueId,
    int MatchId,
    byte LeagueType,
    int BetType,
    string Race,
    bool LiveIndicator,
    string Language,
    string CustName,
    DateTime FromDate,
    DateTime ToDate,
    byte CustLevelId,
    bool DisplayVNIP,
    string SiteName);