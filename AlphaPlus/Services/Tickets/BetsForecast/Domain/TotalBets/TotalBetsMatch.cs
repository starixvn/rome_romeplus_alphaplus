using AlphaPlus.BetsForecast.Domain.Forecast;

namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class TotalBetsMatch : ForecastMatch
{
    public int LeagueId { get; init; }

    public string LeagueName { get; set; }
}
