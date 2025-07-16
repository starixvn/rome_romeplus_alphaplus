using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.CashBillingService.Criteria;

internal class ExtraProductCriteria(int custId, string cashServiceSites) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string CashServiceSites { get; } = cashServiceSites;

    public override string GetSettingKey() => "Age_GetOfferedProductByCust_Extra";

    public override bool IsValid() => CustId > 0;
}
