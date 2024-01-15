namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class AH(bool hasData, decimal[] hdpou) : ForecastMatch
{
    public bool HasData { get; } = hasData;

    public decimal[] HDPOU { get; } = hdpou;
}
