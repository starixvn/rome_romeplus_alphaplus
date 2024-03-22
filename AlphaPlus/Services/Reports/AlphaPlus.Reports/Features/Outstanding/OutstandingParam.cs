namespace AlphaPlus.Reports.Features.Outstanding;

public record OutstandingParam(
    UserLevel CustLevel,
    int CustId,
    string ProductIds,
    bool AutoFollowBetIsDummyAgent,
    string SiteName);