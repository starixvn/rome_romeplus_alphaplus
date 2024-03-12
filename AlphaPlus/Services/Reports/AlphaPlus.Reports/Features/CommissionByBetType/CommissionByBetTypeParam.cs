namespace AlphaPlus.Reports.Features.CommissionByBetType;

public record CommissionByBetTypeParam(
    UserLevel CustLevel,
    int CustId,
    DateTime FromDate,
    DateTime ToDate,
    string SiteName);
