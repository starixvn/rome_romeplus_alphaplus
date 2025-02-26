namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class SoccerResult : SoccerMatch
{
    public IEnumerable<SoccerBetTypeForecast> BetTypeForecasts { get; init; }

    public int TotalBetCount { get; init; }

    public decimal TotalTurnover { get; init; }

    public decimal TotalForecastWinLoss { get; init; }

    public int MatchRowSpan => (BetTypeForecasts.Count() * 2) + 1;
}