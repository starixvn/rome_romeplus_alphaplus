using Fanex.Data.Repository;

namespace AgencyCash.API.Shared.Criteria;

public class GetAgentServiceTypeCriteria(int custId, int custDBGroupId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public byte ServiceType { get; set; }

    public override string GetSettingKey() => $"CustDB{custDBGroupId}_Age_CashService_CustServiceTypeSel";

    public override bool IsValid() => CustId > 0;
}