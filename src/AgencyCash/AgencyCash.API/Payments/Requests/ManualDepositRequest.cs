using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class ManualDepositRequest
{
    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    /// <summary>
    /// 3: PromptPay
    /// 0: Bank Transfer
    /// </summary>
    [Range(0, short.MaxValue, ErrorMessage = "{0} must be between {1} and {2}.")]
    public short PaymentMethodId { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentFrom { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentTo { get; init; }

    [PositiveNumber]
    public decimal Amount { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }
}