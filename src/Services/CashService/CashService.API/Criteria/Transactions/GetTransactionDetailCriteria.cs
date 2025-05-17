namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetTransactionDetailCriteria : CriteriaBase
{
    public long TransId { get; set; }
    public int CustId { get; set; }

    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashTransactionDetail_Sel";

    public override bool IsValid() => TransId > 0 && CustId > 0;
}