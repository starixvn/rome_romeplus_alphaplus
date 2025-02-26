
namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class HomeDrawAwayNoBet : TotalBetsMatch
{
    public decimal Home_Odds_DNB { get; init; }

    public decimal Away_Odds_DNB { get; init; }

    public decimal Home_Odds_HNB { get; init; }

    public decimal Away_Odds_HNB { get; init; }

    public decimal Home_Odds_ANB { get; init; }

    public decimal Away_Odds_ANB { get; init; }

    public int BetCount_DNB { get; init; }

    public int BetCount_HNB { get; init; }

    public int BetCount_ANB { get; init; }
}