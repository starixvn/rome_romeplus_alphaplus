using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingWithdrawal;

public class GetLimitAmtWithdrawCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public int ProviderId { get; set; }

    public int UplineId { get; set; }

    public decimal CashBalance { get; set; }

    public override string GetSettingKey() => "Age_CashWithdrawal_LimitAmtSel";

    public override bool IsValid() => CustId > 0 && ProviderId > 0;
}