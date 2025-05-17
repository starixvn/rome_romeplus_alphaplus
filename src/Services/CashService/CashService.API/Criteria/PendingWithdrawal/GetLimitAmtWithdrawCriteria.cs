using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingWithdrawal;

using Fanex.Data;

public class GetLimitAmtWithdrawCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public int ProviderId { get; set; }

    public int UplineId { get; set; }

    public int PaymentMethodId { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public decimal MinAmt { get; set; }
    public decimal MaxAmt { get; set; }

    public override string GetSettingKey() => $"CustDb_{CustDbGroupId}_Age_CashWithdrawal_LimitAmtSel";

    public override bool IsValid() => CustId > 0 && ProviderId > 0;
}