using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Withdrawal;

public class GetMemberCashBalanceCriteria(int custId, short siteDBGroupId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    [SpParam(Ignored = true)]
    public short SiteDBGroupId { get; } = siteDBGroupId;

    public decimal TotalCash { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("Age_CashService_CashBalance_Sel", SiteDBGroupId);

    public override bool IsValid() => CustId > 0;
}