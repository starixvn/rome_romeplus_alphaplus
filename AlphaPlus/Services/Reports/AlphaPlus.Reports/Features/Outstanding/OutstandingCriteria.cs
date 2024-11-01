using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Outstanding;

internal class OutstandingCriteria(byte custLevelId, int custId, string productIds) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string ProductIds { get; } = productIds;

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(custLevelId)
        ? "Age_Rpt_Outstanding_Member"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_Rpt_Outstanding_Agent"
            : "Age_Rpt_Outstanding_Master";

    public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(ProductIds);
}