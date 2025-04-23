using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.RunningBetListByBetType;

internal class GetBetListRunningByBetTypeCriteria(int custId, int betType, byte custLevelId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(custLevelId)
        ? "Age_BetList_SelRunningByBettype_Ag"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_BetList_SelRunningByBettype_MAg"
            : "Age_BetList_SelRunningByBettype_SAg";

    public override bool IsValid() => CustId > 0 && BetType > 0;
}