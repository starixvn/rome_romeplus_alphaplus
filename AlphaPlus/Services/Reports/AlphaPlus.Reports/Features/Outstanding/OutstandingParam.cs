namespace AlphaPlus.Reports.Features.Outstanding;

public record OutstandingParam(
    byte CustLevelId,
    int CustId,
    string ProductIds,
    bool AutoFollowBetIsDummyAgent,
    string SiteName);