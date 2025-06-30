using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Deposit;

public class AddTransactionDepositCriteria(
    int agentId,
    int custId,
    short siteDBGroupId,
    decimal agentGivenCredit,
    decimal creditMembersGivenCredit,
    decimal minAmount,
    decimal maxAmount,
    byte transactionTypeId,
    byte paymentType,
    short paymentMethodId,
    string paymentFrom,
    string paymentTo,
    decimal amount,
    decimal? userInputAmount = null,
    byte? matchingMode = null,
    short? providerId = null,
    string ip = null,
    string remark = null) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public short SiteDBGroupId { get; } = siteDBGroupId;

    public decimal AgentGivenCredit { get; } = agentGivenCredit;

    public decimal CreditMembers_GivenCredit { get; } = creditMembersGivenCredit;

    public decimal MinAmt { get; } = minAmount;

    public decimal MaxAmt { get; } = maxAmount;

    public byte TransactionTypeId { get; } = transactionTypeId;

    public byte PaymentType { get; } = paymentType;

    public short PaymentMethodId { get; } = paymentMethodId;

    public string PaymentFrom { get; } = paymentFrom;

    public string PaymentTo { get; } = paymentTo;

    public decimal ExchangeRate { get; }

    public decimal Amount { get; } = amount;

    public decimal? UserInput_Amount { get; } = userInputAmount;

    public byte? MatchingMode { get; } = matchingMode;

    public short? ProviderId { get; } = providerId;

    public string IP { get; } = ip;

    public string Remark { get; } = remark;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string ErrExtraInfo { get; set; }

    public long TransId { get; set; }

    public string CashAccumCreditTbl { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("API_CashDeposit_AddTransaction", SiteDBGroupId);

    public override bool IsValid() => AgentId > 0 && CustId > 0 && Amount > 0;
}