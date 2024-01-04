namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class MoneyLine : TotalBetsMatch
{
    public decimal Home_Odds { get; init; }

    public decimal Away_Odds { get; init; }

    public decimal WinLost_Home { get; init; }

    public decimal WinLost_Away { get; init; }

    public decimal Home_Odds_HT { get; init; }

    public decimal Away_Odds_HT { get; init; }

    public decimal WinLost_Home_HT { get; init; }

    public decimal WinLost_Away_HT { get; init; }
}