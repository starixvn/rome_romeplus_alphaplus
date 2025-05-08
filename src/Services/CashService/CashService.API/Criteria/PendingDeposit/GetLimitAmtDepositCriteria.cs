using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit;

public class GetLimitAmtDepositCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public decimal AccumCreditReturn { get; set; }

    public override string GetSettingKey() => "Age_CashDeposit_LimitAmtSel";

    public override bool IsValid() => true;
}