using System.ComponentModel.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class ManualDepositRequest
{
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int AgentId { get; init; }

    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int CustId { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentFrom { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentTo { get; init; }

    [Range(minimum: 0, maximum: double.MaxValue, MinimumIsExclusive = true, ErrorMessage = "{0} must be between {1} and {2}.")]
    public decimal Amount { get; init; }

    [Length(0, 1024 * 1024)]
    public byte[] PaySlip { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }
}