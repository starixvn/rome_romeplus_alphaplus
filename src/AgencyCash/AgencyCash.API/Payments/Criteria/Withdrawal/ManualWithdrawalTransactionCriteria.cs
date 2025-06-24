using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Withdrawal;

public class ManualWithdrawalTransactionCriteria : NonQueryCommand
{
    public ManualWithdrawalTransactionCriteria(
        int custId,
        int agentId,
        short siteDBGroupId,
        byte transactionTypeId,
        decimal minAmt,
        decimal maxAmt,
        short dailyWithdrawalCnt_Setting,
        string paymentTo,
        decimal amount,
        string ip)
    {
        CustId = custId;
        AgentId = agentId;
        SiteDBGroupId = siteDBGroupId;
        TransactionTypeId = transactionTypeId;
        MinAmt = minAmt;
        MaxAmt = maxAmt;
        DailyWithdrawalCnt_Setting = dailyWithdrawalCnt_Setting;
        PaymentTo = paymentTo;
        Amount = amount;
        IP = ip;
    }

    public int CustId { get; }
    public int AgentId { get; }
    public short SiteDBGroupId { get; }
    public byte TransactionTypeId { get; }
    public decimal MinAmt { get; }
    public decimal MaxAmt { get; }
    public short DailyWithdrawalCnt_Setting { get; }
    public string PaymentTo { get; }
    public decimal Amount { get; }
    public string IP { get; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string ErrExtraInfo { get; set; }
    public long TransId { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("API_CashWithdrawal_AddTransaction", SiteDBGroupId);

    public override bool IsValid() => CustId > 0 && AgentId > 0 && Amount > 0 && !string.IsNullOrWhiteSpace(PaymentTo);
}