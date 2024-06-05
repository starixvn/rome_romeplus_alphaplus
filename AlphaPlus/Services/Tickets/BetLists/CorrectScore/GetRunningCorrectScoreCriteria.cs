using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CorrectScore;

public class GetRunningCorrectScoreCriteria(int custId, UserLevel custLevel) : CriteriaBase
{
    public int CustId { get; } = custId;

    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public override string GetSettingKey()
        => CustLevel.IsAgentLevel()
        ? "Age_TotalBets_Sel430_Ag"
        : CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel430_MAg"
            : "Age_TotalBets_Sel430_SAg";

    public override bool IsValid() => CustId > 0;
}
