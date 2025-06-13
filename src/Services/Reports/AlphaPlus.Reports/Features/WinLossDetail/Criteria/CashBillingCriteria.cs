using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossDetail.Criteria;

internal class CashBillingCriteria(int custId, DateTime fromDate, DateTime toDate, DateTime balanceUpDay, DateTime moveBetDay) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime BalanceUpDay { get; } = balanceUpDay;

    public DateTime MoveBetDay { get; } = moveBetDay;

    public override string GetSettingKey() => "Age_Rpt_CashServiceBilling";

    public override bool IsValid() => true;
}
