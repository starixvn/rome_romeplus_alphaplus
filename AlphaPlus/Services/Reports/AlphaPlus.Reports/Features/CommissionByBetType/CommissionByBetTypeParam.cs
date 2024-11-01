namespace AlphaPlus.Reports.Features.CommissionByBetType;

public record CommissionByBetTypeParam(
    byte CustLevelId,
    int CustId,
    DateTime FromDate,
    DateTime ToDate,
    string SiteName);
