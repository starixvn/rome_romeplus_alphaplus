using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Withdrawal;

public class GetDailyWithdrawalCriteria : NonQueryCommand
{
    public GetDailyWithdrawalCriteria(int custId, short siteDBGroupId)
    {
        CustId = custId;
        SiteDBGroupId = siteDBGroupId;
    }

    public int CustId { get; }
    [SpParam(Ignored = true)]
    public short SiteDBGroupId { get; }

    public int WithdrawalCnt { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("Age_CashMember_DailyWithdrawalCount_Sel", SiteDBGroupId);

    public override bool IsValid() => CustId > 0;
}