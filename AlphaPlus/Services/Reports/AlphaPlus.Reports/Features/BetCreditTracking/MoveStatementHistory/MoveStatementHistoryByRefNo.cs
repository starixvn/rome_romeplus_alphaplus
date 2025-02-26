namespace AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

public class MoveStatementHistoryByRefNo
{
    public int CustId { get; init; }

    public string Username { get; init; }

    public long TransId { get; init; }

    public DateTime OldEventDate { get; init; }

    public DateTime NewEventDate { get; init; }

    public decimal MemberWinLoss { get; init; }

    public decimal MemberComm { get; init; }

    public decimal MemberTotal { get; init; }

    public bool IsMultipleMoved { get; init; }
}