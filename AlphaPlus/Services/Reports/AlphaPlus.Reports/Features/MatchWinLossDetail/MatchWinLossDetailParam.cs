namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public record MatchWinLossDetailParam(
    int ViewerId,
    UserLevel ViewerLevel,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string LeagueIds,
    string SiteName);