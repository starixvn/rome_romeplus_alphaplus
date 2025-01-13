namespace AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

public class MoveStatementHistoryByMember(
    string username,
    MoveStatementHistoryByRefNo memberRow,
    IEnumerable<MoveStatementHistoryByRefNo> refNoRow)
{
    public MoveStatementHistoryByRefNo MemberRow { get; }
        = new MoveStatementHistoryByRefNo
        {
            Username = username,
            MemberWinLoss = memberRow.MemberWinLoss,
            MemberComm = memberRow.MemberComm,
            MemberTotal = memberRow.MemberTotal,
            IsDirectMember = memberRow.IsDirectMember
        };

    public IEnumerable<MoveStatementHistoryByRefNo> RefNoRow { get; } = refNoRow;
}
