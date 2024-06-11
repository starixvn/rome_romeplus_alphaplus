namespace AlphaPlus.BetLists.WinLossByMatch;

public record WinLossByMatchParam(
    int CustId,
    int LeagueId,
    int MatchId,
    int BetType,
    string Race,
    bool LiveIndicator,
    string Language,
    string CustName,
    DateTime FromDate,
    DateTime ToDate,
    byte CustLevelId,
    bool DisplayVNIP,
    string SiteName)
{
    public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);
}