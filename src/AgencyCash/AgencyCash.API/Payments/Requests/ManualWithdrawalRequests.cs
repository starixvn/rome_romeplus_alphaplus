using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class ManualWithdrawalRequest
{
    [PositiveNumber]
    public int CustId { get; set; }

    [PositiveNumber]
    public int AgentId { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentTo { get; set; }

    [PositiveNumber]
    public decimal Amount { get; set; }

    [StringLength(maximumLength: 50, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string IP { get; set; }
}