namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class SoccerMatch : Match
{
    public string ShowTime { get; init; }

    public bool IsLive { get; init; }

    public int LiveHomeScore { get; init; }

    public int LiveAwayScore { get; init; }

    public bool HasLive { get; init; }

    public DateTime KickoffTime { get; init; }
}
