using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Outstanding;

public class OutstandingCriteria(UserLevel custLevel, int custId, string productIds) : CriteriaBase
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public int CustId { get; } = custId;

    public string ProductIds { get; } = productIds;

    public override string GetSettingKey()
        => CustLevel.IsAgentLevel()
        ? "Age_Rpt_Outstanding_Member"
        : CustLevel.IsMasterLevel()
            ? "Age_Rpt_Outstanding_Agent"
            : "Age_Rpt_Outstanding_Master";

    public override bool IsValid() => CustLevel.IsNotMemberLevel();
}