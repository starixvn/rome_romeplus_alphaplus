namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ForecastMatch : Match
{
    public string ShowTime { get; init; }

    public bool LiveIndicator { get; init; }

    public bool IsNeutral { get; init; }

    public new string Event => $"{HomeName} -vs- {AwayName}";

    public int LiveHomeScore { get; init; }

    public int LiveAwayScore { get; init; }
}