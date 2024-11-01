using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CorrectScore;

internal class GetRunningCorrectScoreCriteria(int custId, byte custLevelId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(custLevelId)
        ? "Age_TotalBets_Sel430_Ag"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_TotalBets_Sel430_MAg"
            : "Age_TotalBets_Sel430_SAg";

    public override bool IsValid() => CustId > 0;
}
