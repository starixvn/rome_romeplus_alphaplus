using Fanex.Data.Repository;

namespace AlphaPlus.PaymentService.API.Shared.Criteria;

public class GetSiteDBConfigCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Acc_ServerDBConfigSelByCust";

    public override bool IsValid() => CustId > 0;
}