namespace AlphaPlus.Reports.Features.Outstanding;

public record OutstandingParam(
    UserLevel UserLevel,
    UserLevel CustLevel,
    int CustId,
    string ProductIds,
    bool AutoFollowBetIsDummyAgent,
    string Site);