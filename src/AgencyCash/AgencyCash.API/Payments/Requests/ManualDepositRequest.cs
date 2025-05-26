using System.ComponentModel.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class ManualDepositRequest
{
    [Range(minimum: 1, maximum: int.MaxValue)]
    public int AgentId { get; init; }

    [Range(minimum: 1, maximum: int.MaxValue)]
    public int CustId { get; init; }

    [Required, StringLength(maximumLength: 100)]
    public string PaymentFrom { get; init; }

    [Required, StringLength(maximumLength: 100)]
    public string PaymentTo { get; init; }

    [Range(minimum: 0, maximum: double.MaxValue, MinimumIsExclusive = true)]
    public decimal Amount { get; init; }

    [Length(0, 1024 * 1024)]
    public byte[] PaySlip { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }
}