using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Parlay;

public class GetParlayBetListCriteria(long transId, int custId) : CriteriaBase
{
    public long TransId { get; } = transId;

    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Age_BetList_SelParlayDetail";

    public override bool IsValid() => TransId > 0 && CustId > 0;
}