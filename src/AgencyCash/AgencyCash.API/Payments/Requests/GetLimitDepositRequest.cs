using System.ComponentModel.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class GetLimitDepositRequest
{
    private const string RangeErrorMessage = "{0} must be between {1} and {2}.";

    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = RangeErrorMessage)]
    public int AgentId { get; init; }

    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = RangeErrorMessage)]
    public int CustId { get; init; }

    [Range(minimum: 1, maximum: 2, ErrorMessage = RangeErrorMessage)]
    public byte PaymentType { get; init; }

    [Range(minimum: 1, maximum: short.MaxValue, ErrorMessage = RangeErrorMessage)]
    public short ProviderId { get; init; }

    [Range(minimum: 1, maximum: short.MaxValue, ErrorMessage = RangeErrorMessage)]
    public short PaymentMethodId { get; init; }
}