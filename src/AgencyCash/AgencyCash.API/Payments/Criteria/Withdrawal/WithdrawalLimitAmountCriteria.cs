using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Withdrawal;

public class WithdrawalLimitAmountCriteria : NonQueryCommand
{
    public WithdrawalLimitAmountCriteria(
        int custId,
        int uplineId,
        bool isMemberSite,
        short custDBGroupId,
        int? withdrawalCnt = null)
    {
        CustId = custId;
        UplineId = uplineId;
        IsMemberSite = isMemberSite;
        CustDBGroupId = custDBGroupId;
        WithdrawalCnt = withdrawalCnt;
    }

    public int CustId { get; }
    public int UplineId { get; }
    public bool IsMemberSite { get; }
    [SpParam(Ignored = true)]
    public short CustDBGroupId { get; }
    public int? WithdrawalCnt { get; }

    public decimal MinAmt { get; set; }
    public decimal MaxAmt { get; set; }
    public short DailyWithdrawalCnt_Setting { get; set; }
    public int WithdrawalLimitCnt { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashWithdrawal_LimitAmtSel", CustDBGroupId);

    public override bool IsValid() => CustId > 0 && UplineId > 0;
}