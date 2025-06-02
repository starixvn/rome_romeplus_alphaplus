using Fanex.Data.Repository;

namespace AgencyCash.API.Shared.Criteria;

public class GetSiteDBConfigCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Acc_ServerDBConfigSelByCust";

    public override bool IsValid() => CustId > 0;
}