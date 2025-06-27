using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Withdrawal;

public class ManualWithdrawalTransactionCriteria(
    int custId,
    int agentId,
    short siteDBGroupId,
    byte transactionTypeId,
    decimal minAmt,
    decimal maxAmt,
    short dailyWithdrawalCnt_Setting,
    string paymentTo,
    decimal amount,
    string ip) : NonQueryCommand
{
    public int CustId { get; } = custId;
    public int AgentId { get; } = agentId;
    public short SiteDBGroupId { get; } = siteDBGroupId;
    public byte TransactionTypeId { get; } = transactionTypeId;
    public decimal MinAmt { get; } = minAmt;
    public decimal MaxAmt { get; } = maxAmt;
    public short DailyWithdrawalCnt_Setting { get; } = dailyWithdrawalCnt_Setting;
    public string PaymentTo { get; } = paymentTo;
    public decimal Amount { get; } = amount;
    public string IP { get; } = ip;

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string ErrExtraInfo { get; set; }
    public long TransId { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("API_CashWithdrawal_AddTransaction", SiteDBGroupId);

    public override bool IsValid() => CustId > 0 && AgentId > 0 && Amount > 0 && !string.IsNullOrWhiteSpace(PaymentTo);
}