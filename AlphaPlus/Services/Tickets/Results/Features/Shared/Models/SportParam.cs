namespace AlphaPlus.Results.Features.Shared;

public record SportParam(DateTime EventDate, bool IsShowAll, string SiteName, IDictionary<int, bool> SportsStatus);