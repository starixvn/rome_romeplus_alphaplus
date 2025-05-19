using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class GetDepositMethodsCriteria(int agentId) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public override string GetSettingKey()
    {
        throw new NotImplementedException();
    }

    public override bool IsValid() => AgentId > 0;
}