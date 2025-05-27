using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class ManualDepositRequest
{
    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentFrom { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentTo { get; init; }

    [PositiveNumber]
    public decimal Amount { get; init; }

    [Length(0, 1024 * 1024, ErrorMessage = "{0} cannot exceed 1MB.")]
    public byte[] PaySlip { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }
}