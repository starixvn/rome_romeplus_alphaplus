using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetSoccerResultRequest : RequestBase
{
    public bool IsLive { get; init; } = true;

    public string MatchIds { get; init; }

    public bool IsValid()
        => !string.IsNullOrWhiteSpace(MatchIds)
        && MatchIds.Split(',', StringSplitOptions.RemoveEmptyEntries).All(m => int.TryParse(m, out _));
}
