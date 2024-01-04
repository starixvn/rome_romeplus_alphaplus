namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class OE1x2DND : TotalBetsMatch
{
    public decimal Home_Odds_OE { get; init; }

    public decimal Away_Odds_OE { get; init; }

    public decimal Home_Odds_OE_1st { get; init; }

    public decimal Away_Odds_OE_1st { get; init; }

    public decimal Odds_1 { get; init; }

    public decimal Odds_X { get; init; }

    public decimal Odds_2 { get; init; }

    public decimal Odds_1_1st { get; init; }

    public decimal Odds_X_1st { get; init; }

    public decimal Odds_2_1st { get; init; }

    public decimal Home_Odds_DND { get; init; }

    public decimal Away_Odds_DND { get; init; }

    public int BetCount_OE { get; init; }

    public int BetCount_OE_1st { get; init; }

    public int BetCount_1x2 { get; init; }

    public int BetCount_1x2_1st { get; init; }

    public int BetCount_DND { get; init; }
}