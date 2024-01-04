namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class NumberGame
{
    public string ShowTime { get; init; }

    public int LeagueId { get; init; }

    public string LeagueName { get; set; }

    public string MatchCode { get; init; }

    public int MatchId { get; init; }

    public decimal Over_1stBall { get; init; }

    public decimal Under_1stBall { get; init; }

    public decimal Over_LastBall { get; init; }

    public decimal Under_LastBall { get; init; }

    public decimal Odd_1stBall { get; init; }

    public decimal Even_1stBall { get; init; }

    public decimal Odd_LastBall { get; init; }

    public decimal Even_LastBall { get; init; }

    public decimal Odd_FullTime { get; init; }

    public decimal Even_FullTime { get; init; }

    public decimal Over_Next { get; init; }

    public decimal Under_Next { get; init; }

    public decimal Odd_Next { get; init; }

    public decimal Even_Next { get; init; }

    public decimal NextCombo_OverOdd { get; init; }

    public decimal NextCombo_OverEven { get; init; }

    public decimal NextCombo_UnderOdd { get; init; }

    public decimal NextCombo_UnderEven { get; init; }

    public decimal NumberGroupPre { get; init; }

    public decimal NumberGroupLive { get; init; }

    public decimal NextRB_R { get; init; }

    public decimal NextRB_B { get; init; }

    public int BetCount_OU1stBall { get; init; }

    public int BetCount_OULastBall { get; init; }

    public int BetCount_OE1stBall { get; init; }

    public int BetCount_OELastBall { get; init; }

    public int BetCount_OEFullTime { get; init; }

    public int BetCount_OUNext { get; init; }

    public int BetCount_OENext { get; init; }

    public int BetCount_NextCombo_OverOdd { get; init; }

    public int BetCount_NextCombo_OverEven { get; init; }

    public int BetCount_NextCombo_UnderOdd { get; init; }

    public int BetCount_NextCombo_UnderEven { get; init; }

    public int BetCount_NumberGroupPre { get; init; }

    public int BetCount_NumberGroupLive { get; init; }

    public int BetCount_NextRB { get; init; }
}