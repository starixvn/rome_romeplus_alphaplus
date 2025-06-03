using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossDetail.Criteria;

public class CashBillingCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_Rpt_CashServiceBilling";

    public override bool IsValid() => true;
}
