using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class ManualWithdrawalCriteria : INonQueryCommand
{
    public int CustId { get; }
    public int AgentId { get; }
    public decimal AgentGivenCredit { get; }
    public decimal CreditMembersGivenCredit { get; }
    public decimal MinLimitAmount { get; }
    public decimal MaxLimitAmount { get; }
    public string PaymentFrom { get; }
    public string PaymentTo { get; }
    public int Status { get; }
    public decimal Amount { get; }
    public string IP { get; }
    public int SiteDBGroupId { get; }
    public string CashAccumCreditTbl { get; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public ManualWithdrawalCriteria(
        int custId,
        int agentId,
        decimal agentGivenCredit,
        decimal creditMembersGivenCredit,
        decimal minLimitAmount,
        decimal maxLimitAmount,
        string paymentFrom,
        string paymentTo,
        int status,
        decimal amount,
        string ip,
        int siteDBGroupId)
    {
        CustId = custId;
        AgentId = agentId;
        AgentGivenCredit = agentGivenCredit;
        CreditMembersGivenCredit = creditMembersGivenCredit;
        MinLimitAmount = minLimitAmount;
        MaxLimitAmount = maxLimitAmount;
        PaymentFrom = paymentFrom;
        PaymentTo = paymentTo;
        Status = status;
        Amount = amount;
        IP = ip;
        SiteDBGroupId = siteDBGroupId;
    }

    public string GetSettingKey()
    {
        return $"Withdrawal_{CustId}_{AgentId}";
    }

    public bool IsValid()
    {
        if (CustId <= 0 || AgentId <= 0)
        {
            ErrCode = 400;
            ErrMsg = "Invalid customer or agent ID";
            return false;
        }

        if (Amount < MinLimitAmount || Amount > MaxLimitAmount)
        {
            ErrCode = 400;
            ErrMsg = "Amount is outside the allowed limits";
            return false;
        }

        return true;
    }
}