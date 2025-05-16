using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit;

public class GetAccumulateCreditCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public decimal AccumCreditReturn { get; set; }

    public override string GetSettingKey() => "Age_CashAccumCredit_AgSel";

    public override bool IsValid() => CustId > 0;
}