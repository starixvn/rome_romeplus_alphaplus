namespace AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

public record MoveStatementHistoryParam(
    string Username,
    DateTime FromDate,
    DateTime ToDate,
    int ViewerId,
    int PageIndex,
    int PageSize,
    bool IsVariance,
    string SiteName);
