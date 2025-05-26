using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;

public class GetLimitAmtDepositCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public decimal AccumCreditReturn { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public decimal MinAmt { get; set; }

    public decimal MaxAmt { get; set; }

    public override string GetSettingKey() => $"CustDb_{CustDbGroupId}_Age_CashDeposit_LimitAmtSel";

    public override bool IsValid() => CustId > 0;
}