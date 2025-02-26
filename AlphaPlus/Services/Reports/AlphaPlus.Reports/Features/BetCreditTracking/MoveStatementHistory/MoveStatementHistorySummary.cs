namespace AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

public class MoveStatementHistorySummary
{
    public int TotalMembers { get; init; }

    public decimal MemberWinLoss { get; init; }

    public decimal MemberComm { get; init; }

    public decimal MemberTotal { get; init; }

    public decimal Variance { get; init; }
}