using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Withdrawal;

public class WithdrawalLimitAmountCriteria(
    int custId,
    int uplineId,
    bool isMemberSite,
    short custDBGroupId,
    int? withdrawalCnt = null) : NonQueryCommand
{
    public int CustId { get; } = custId;
    public int UplineId { get; } = uplineId;
    public bool IsMemberSite { get; } = isMemberSite;
    [SpParam(Ignored = true)]
    public short CustDBGroupId { get; } = custDBGroupId;
    public int? WithdrawalCnt { get; } = withdrawalCnt;

    public decimal MinAmt { get; set; }
    public decimal MaxAmt { get; set; }
    public short DailyWithdrawalCnt_Setting { get; set; }
    public int WithdrawalLimitCnt { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashWithdrawal_LimitAmtSel", CustDBGroupId);

    public override bool IsValid() => CustId > 0 && UplineId > 0;
}