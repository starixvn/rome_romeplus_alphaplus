using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.MoveStatementHistory;

public class GetBetListMoveStatementHistoryCriteria(int custId, long transId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public long TransId { get; } = transId;

    public override string GetSettingKey() => "Age_BetList_SelMovingEventByTransId";

    public override bool IsValid() => CustId > 0 && TransId > 0;
}