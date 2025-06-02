using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Deposit;

public class GetAgentCashAccumCreditCriteria(int custId, short siteDBGroupId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public decimal AccumCreditReturn { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("Age_CashAccumCredit_AgSel", siteDBGroupId);

    public override bool IsValid() => CustId > 0;
}