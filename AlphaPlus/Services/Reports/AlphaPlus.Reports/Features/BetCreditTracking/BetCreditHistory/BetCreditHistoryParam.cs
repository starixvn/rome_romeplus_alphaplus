namespace AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

public record BetCreditHistoryParam(
    string Member,
    long? RefNo,
    DateTime FromDate,
    DateTime ToDate,
    int ViewerId,
    string Language,
    int PageIndex,
    int PageSize,
    string SiteName);
