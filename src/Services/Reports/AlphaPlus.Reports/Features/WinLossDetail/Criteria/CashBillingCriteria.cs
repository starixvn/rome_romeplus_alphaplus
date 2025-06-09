using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossDetail.Criteria;

internal class CashBillingCriteria(int custId, DateTime fromDate, DateTime toDate) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public override string GetSettingKey() => "Age_Rpt_CashServiceBilling";

    public override bool IsValid() => true;
}
