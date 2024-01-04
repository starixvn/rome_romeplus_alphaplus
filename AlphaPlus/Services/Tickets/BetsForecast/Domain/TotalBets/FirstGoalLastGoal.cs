namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class FirstGoalLastGoal : TotalBetsMatch
{
    public decimal _14_WinLost_HomeFG { get; init; }

    public decimal _14_WinLost_HomeLG { get; init; }

    public decimal _14_WinLost_AwayFG { get; init; }

    public decimal _14_WinLost_AwayLG { get; init; }

    public decimal _14_WinLost_NoGoal { get; init; }

    public decimal _127_WinLost_HomeFG { get; init; }

    public decimal _127_WinLost_HomeLG { get; init; }

    public decimal _127_WinLost_AwayFG { get; init; }

    public decimal _127_WinLost_AwayLG { get; init; }

    public decimal _127_WinLost_NoGoal { get; init; }

    public int BetCount_14 { get; init; }

    public int BetCount_127 { get; init; }
}