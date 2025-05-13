namespace AlphaPlus.CashService.API.Criteria.Common;

using Fanex.Data.Repository;

public class GetSiteDBGroupIDCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public override string GetSettingKey() => "Acc_ServerDBConfigSelByCust";

    public override bool IsValid() => CustId > 0;
}