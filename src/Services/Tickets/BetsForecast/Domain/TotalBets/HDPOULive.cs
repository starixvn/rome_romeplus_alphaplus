namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class HDPOULive : TotalBetsMatch
{
    public decimal Home_Odds_HDP { get; init; }

    public decimal Away_Odds_HDP { get; init; }

    public decimal Total_Odds_HDP => Home_Odds_HDP - Away_Odds_HDP;

    public decimal Home_Odds_HTHDP { get; init; }

    public decimal Away_Odds_HTHDP { get; init; }

    public decimal Total_Odds_HTHDP => Home_Odds_HTHDP - Away_Odds_HTHDP;

    public decimal Home_Odds_OU { get; init; }

    public decimal Away_Odds_OU { get; init; }

    public decimal Total_Odds_OU => Home_Odds_OU - Away_Odds_OU;

    public decimal Home_Odds_HTOU { get; init; }

    public decimal Away_Odds_HTOU { get; init; }

    public decimal Total_Odds_HTOU => Home_Odds_HTOU - Away_Odds_HTOU;

    public int BetCount_HDP { get; init; }

    public int BetCount_HTHDP { get; init; }

    public int BetCount_OU { get; init; }

    public int BetCount_HTOU { get; init; }
}
