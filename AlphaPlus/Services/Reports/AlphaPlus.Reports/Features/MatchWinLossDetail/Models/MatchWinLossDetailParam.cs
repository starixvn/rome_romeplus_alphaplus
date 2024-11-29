namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public record MatchWinLossDetailParam(
    int ViewerId,
    byte ViewerLevelId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string LeagueIds,
    string League3rdIds,
    string SiteName);