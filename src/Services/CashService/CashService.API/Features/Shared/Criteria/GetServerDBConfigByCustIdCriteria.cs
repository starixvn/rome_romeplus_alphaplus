namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

using Fanex.Data.Repository;

public class GetServerDBConfigByCustIdCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public override string GetSettingKey() => "Acc_ServerDBConfigSelByCust";

    public override bool IsValid() => CustId > 0;
}