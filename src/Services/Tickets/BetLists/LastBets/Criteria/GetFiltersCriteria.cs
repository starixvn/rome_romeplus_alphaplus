using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

internal class GetFiltersCriteria(int custId, int subAccId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int SubAccId { get; } = subAccId;

    public override string GetSettingKey() => "Age_CustLastBetFilter_Sel";

    public override bool IsValid() => CustId > 0 && SubAccId >= 0;
}