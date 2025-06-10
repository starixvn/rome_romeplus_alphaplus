using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Deposit;

public class GetDepositLimitCriteria : NonQueryCommand
{
    public GetDepositLimitCriteria(
        int custId,
        byte paymentType,
        short providerId,
        short paymentMethodId,
        decimal accumCreditReturn,
        short custDBGroupId)
    {
        CustId = custId;
        PaymentType = paymentType;
        ProviderId = providerId;
        PaymentMethodId = paymentMethodId;
        AccumCreditReturn = accumCreditReturn;
        CustDBGroupId = custDBGroupId;
    }

    public GetDepositLimitCriteria(
        int agentId,
        int custId,
        byte paymentType,
        short providerId = 0,
        short paymentMethodId = 0,
        short custDBGroupId = 0)
    {
        AgentId = agentId;
        CustId = custId;
        PaymentType = paymentType;
        ProviderId = providerId;
        PaymentMethodId = paymentMethodId;
        CustDBGroupId = custDBGroupId;
    }

    public GetDepositLimitCriteria(int agentId, int custId, short custDBGroupId)
    {
        AgentId = agentId;
        CustId = custId;
        CustDBGroupId = custDBGroupId;
    }

    public int? AgentId { get; }

    public int CustId { get; }

    public byte PaymentType { get; }

    public short ProviderId { get; }

    public short PaymentMethodId { get; }

    public decimal AccumCreditReturn { get; }

    public bool IsMemberSite { get; } = true;

    public decimal MinAmt { get; set; }

    public decimal MaxAmt { get; set; }

    public decimal AgentAvailableCredit { get; set; }

    public decimal AgentGivenCredit { get; set; }

    public decimal CreditMembers_GivenCredit { get; set; }

    [SpParam(Ignored = true)]
    public short CustDBGroupId { get; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashDeposit_LimitAmtSel", CustDBGroupId);

    public override bool IsValid() => CustId > 0;
}