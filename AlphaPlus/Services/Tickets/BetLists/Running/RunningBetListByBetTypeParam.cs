namespace AlphaPlus.BetLists.Running;

public record RunningBetListByBetTypeParam(
    int CustId,
    byte CustLevelId,
    int BetType,
    bool DisplayVNIP,
    string SiteName);
