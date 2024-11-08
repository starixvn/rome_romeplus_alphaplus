using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Outstanding;

internal class GetFollowBetAccountsCriteria(int agentId) : CriteriaBase
{
    public int DummyAgentId { get; } = agentId;

    public override string GetSettingKey() => "Age_Customer_FollowBetAccountListSelByAgent";

    public override bool IsValid() => DummyAgentId > 0;
}