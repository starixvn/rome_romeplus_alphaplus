namespace AlphaPlus.Results.Features.Outright;

public record OutrightParam(
    DateTime FromDate,
    DateTime ToDate,
    bool IsShowAll,
    int SportType,
    int LeagueId,
    string SiteName);
