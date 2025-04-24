namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class SoccerBetTypeForecast
{
    public int BetType { get; init; }

    public string BetTypeName { get; set; }

    public int BetCount { get; init; }

    public decimal Turnover { get; init; }

    public IEnumerable<string> WinLossColumnNames { get; init; }

    public IEnumerable<decimal> WinLossValues { get; init; }

    public int MainWinLossIndex { get; set; }

    public bool IsHandicap { get; init; }

    public bool IsOverUnder { get; init; }

    public bool Is1X2 { get; init; }
}