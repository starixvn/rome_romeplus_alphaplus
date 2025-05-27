using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class ManualDepositCriteria(
    int agentId,
    int custId,
    decimal agentGivenCredit,
    decimal creditMembersGivenCredit,
    decimal minLimitAmount,
    decimal maxLimitAmount,
    string paymentFrom,
    string paymentTo,
    decimal exchangeRate,
    decimal amount,
    byte[] paySlip,
    string ip,
    int siteDBGroupId) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public decimal AgentGivenCredit { get; } = agentGivenCredit;

    public decimal CreditMembers_GivenCredit { get; } = creditMembersGivenCredit;

    public decimal MinLimitAmt { get; } = minLimitAmount;

    public decimal MaxLimitAmt { get; } = maxLimitAmount;

    public string PaymentFrom { get; } = paymentFrom;

    public string PaymentTo { get; } = paymentTo;

    public decimal ExchangeRate { get; } = exchangeRate;

    public decimal Amount { get; } = amount;

    public byte[] PaySlip { get; } = paySlip;

    public string IP { get; } = ip;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public long TransId { get; set; }

    public string CashAccumCreditTbl { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("", siteDBGroupId);

    public override bool IsValid() => CustId > 0;
}