namespace AlphaPlus.BetLists.ParlayWinLossByMatch;

public record ParlayWinLossByMatchParam(
    int CustId,
    int BetType,
    string Language,
    DateTime FromDate,
    DateTime ToDate,
    byte CustLevelId,
    bool DisplayVNIP,
    int SuperId,
    string SiteName);