using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Withdrawal;

public class GetMemberCashBalanceCriteria : NonQueryCommand
{
    public GetMemberCashBalanceCriteria(int custId, short siteDBGroupId)
    {
        CustId = custId;
        SiteDBGroupId = siteDBGroupId;
    }

    public int CustId { get; }

    [SpParam(Ignored = true)]
    public short SiteDBGroupId { get; }

    public decimal TotalCash { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("Age_CashService_CashBalance_Sel", SiteDBGroupId);

    public override bool IsValid() => CustId > 0;
}