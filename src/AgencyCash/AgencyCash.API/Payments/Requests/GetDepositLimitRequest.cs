using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class GetDepositLimitRequest
{
    private const byte GatewayPaymentType = 1;

    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    /// <summary>
    /// 1: Gateway
    /// 2: Bank Account
    /// </summary>
    [PositiveNumber]
    public byte PaymentType { get; init; }

    [RequiredIf(nameof(PaymentType), GatewayPaymentType)]
    public short? ProviderId { get; init; }

    [RequiredIf(nameof(PaymentType), GatewayPaymentType)]
    [PositiveNumber]
    public short? PaymentMethodId { get; init; }
}