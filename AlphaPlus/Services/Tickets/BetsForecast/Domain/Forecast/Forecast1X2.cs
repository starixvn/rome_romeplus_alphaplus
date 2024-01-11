namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class Forecast1X2 : ForecastMatch
{
    public decimal? AL { get; init; }

    public decimal? HL { get; init; }

    public decimal? DL { get; init; }

    public decimal? HT_AL { get; init; }

    public decimal? HT_HL { get; init; }

    public decimal? HT_DL { get; init; }
}