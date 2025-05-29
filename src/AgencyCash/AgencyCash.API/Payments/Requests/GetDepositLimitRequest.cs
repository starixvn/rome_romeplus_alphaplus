using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class GetDepositLimitRequest
{
    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    [PositiveNumber]
    public byte PaymentType { get; init; }

    public short ProviderId { get; init; }

    [PositiveNumber]
    public short PaymentMethodId { get; init; }
}