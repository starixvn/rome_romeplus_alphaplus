using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class GetDepositLimitCriteria(
    int agentId,
    int custId,
    byte paymentType,
    short providerId,
    short paymentMethodId) : NonQueryCommand
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public byte PaymentType { get; } = paymentType;

    public short ProviderId { get; } = providerId;

    public short PaymentMethodId { get; } = paymentMethodId;

    public decimal MinAmt { get; set; }

    public decimal MaxAmt { get; set; }

    public override string GetSettingKey()
    {
        throw new NotImplementedException();
    }

    public override bool IsValid() => AgentId > 0 && CustId > 0;
}