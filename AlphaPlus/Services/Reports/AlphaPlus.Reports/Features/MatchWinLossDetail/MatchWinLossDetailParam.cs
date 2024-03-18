namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public record MatchWinLossDetailParam(
    int ViewerId,
    UserLevel ViewerLevel,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    IEnumerable<string> Leagues,
    string SiteName);