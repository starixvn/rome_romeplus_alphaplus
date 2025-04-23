namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ForecastHDPOULive : ForecastMatch
{
    public decimal? WinLost_M1_HDP { get; init; }

    public decimal? WinLost_0_HDP { get; init; }

    public decimal? WinLost_1_HDP { get; init; }

    public decimal? WinLost_0_OU { get; init; }

    public decimal? WinLost_1_OU { get; init; }

    public decimal? WinLost_2_OU { get; init; }

    public decimal? WinLost_M1_HTHDP { get; init; }

    public decimal? WinLost_0_HTHDP { get; init; }

    public decimal? WinLost_1_HTHDP { get; init; }

    public decimal? WinLost_0_HTOU { get; init; }

    public decimal? WinLost_1_HTOU { get; init; }

    public decimal? WinLost_2_HTOU { get; init; }

    public int OffsetHDP => LiveHomeScore - LiveAwayScore;

    public int OffsetOU => LiveHomeScore + LiveAwayScore;
}