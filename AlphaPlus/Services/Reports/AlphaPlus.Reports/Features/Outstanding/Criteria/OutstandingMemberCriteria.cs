using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Outstanding;

internal class OutstandingMemberCriteria(int custId, string productIds) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string ProductIds { get; } = productIds;

    public override string GetSettingKey() => "Age_Rpt_Outstanding_Member";

    public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(ProductIds);
}