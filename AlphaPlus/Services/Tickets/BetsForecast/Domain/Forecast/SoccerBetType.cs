namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class SoccerBetType
{
    public int MatchId { get; init; }

    public int BetTypeId { get; init; }

    public decimal WinLoss1 { get; init; }

    public decimal WinLoss2 { get; init; }

    public decimal WinLoss3 { get; init; }

    public decimal WinLoss4 { get; init; }

    public decimal WinLoss5 { get; init; }
}