using System.ComponentModel.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class AutoDepositRequest
{
    [Range(minimum: 1, maximum: 2)]
    public byte DepositMethod { get; init; }

    [Range(minimum: 1, maximum: int.MaxValue)]
    public int AgentId { get; init; }

    [Range(minimum: 1, maximum: int.MaxValue)]
    public int CustId { get; init; }

    public short ProviderId { get; init; }

    public short PaymentMethodId { get; init; }

    public byte MachingMode { get; init; }

    public string PaymentFrom { get; init; }

    public string PaymentTo { get; init; }

    [Range(minimum: 0, maximum: double.MaxValue, MinimumIsExclusive = true)]
    public decimal Amount { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }
}