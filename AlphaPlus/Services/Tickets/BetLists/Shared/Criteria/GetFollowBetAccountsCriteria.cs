using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class GetFollowBetAccountsCriteria(int agentId) : CriteriaBase
{
    public int DummyAgentId { get; } = agentId;

    public override string GetSettingKey() => "Age_Customer_FollowBetAccountListSelByAgent";

    public override bool IsValid() => DummyAgentId > 0;
}
