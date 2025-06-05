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
        short custDBGroupId)
    {
        CustId = custId;
        UplineId = uplineId;
        IsMemberSite = isMemberSite;
        CustDBGroupId = custDBGroupId;
    }

    public int CustId { get; }
    public int UplineId { get; }
    public bool IsMemberSite { get; }
    [SpParam(Ignored = true)]
    public short CustDBGroupId { get; }

    public decimal MinAmt { get; set; }
    public decimal MaxAmt { get; set; }
    public short DailyWithdrawalCnt_Setting { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashWithdrawal_LimitAmtSel", CustDBGroupId);

    public override bool IsValid() => CustId > 0 && UplineId > 0;
} 