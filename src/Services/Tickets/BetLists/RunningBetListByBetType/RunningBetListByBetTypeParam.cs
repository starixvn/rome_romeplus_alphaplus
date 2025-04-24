namespace AlphaPlus.BetLists.RunningBetListByBetType;

public record RunningBetListByBetTypeParam(
    int CustId,
    byte CustLevelId,
    int BetType,
    bool DisplayVNIP,
    string SiteName);
