namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ScoreMapResultEntity
{
    public int MatchId { get; init; }

    public int Home { get; init; }

    public int Away { get; init; }

    public decimal? HT { get; init; }

    public decimal? FT { get; init; }
}
