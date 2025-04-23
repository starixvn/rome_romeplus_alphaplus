namespace AlphaPlus.Reports.Features.Outstanding;

public record OutstandingParam(
    byte CustLevelId,
    int CustId,
    string ProductIds,
    byte LevelType,
    bool AutoFollowBetIsDummyAgent,
    string SiteName);