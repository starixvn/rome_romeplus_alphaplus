using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Running;

public class GetBetListRunningByBetTypeCriteria(int custId, int betType, UserLevel custLevel) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public override string GetSettingKey()
        => CustLevel.IsAgentLevel()
        ? "Age_BetList_SelRunningByBettype_Ag"
        : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelRunningByBettype_MAg"
            : "Age_BetList_SelRunningByBettype_SAg";

    public override bool IsValid() => CustId > 0 && BetType > 0;
}