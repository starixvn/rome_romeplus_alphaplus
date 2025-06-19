using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Payments.Constants;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class AutoDepositRequest
{
    [Range(AutoDepositType.Gateway, AutoDepositType.DecimalBankAccount, ErrorMessage = "{0} must be {1} (Payment Gateway) or {2} (Decimal or Bank Account).")]
    public byte DepositMethod { get; init; }

    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    [RequiredIf(nameof(DepositMethod), AutoDepositType.Gateway)]
    [PositiveNumber]
    public short? ProviderId { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [Range(0, short.MaxValue, ErrorMessage = "{0} must be between {1} and {2}.")]
    public short? PaymentMethodId { get; init; }

    [RequiredIf(nameof(DepositMethod), AutoDepositType.DecimalBankAccount)]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentFrom { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentTo { get; init; }

    [PositiveNumber]
    public decimal Amount { get; init; }

    [RequiredIf(nameof(MatchingMode), (byte)1)]
    [PositiveNumber]
    public decimal? UserInputAmount { get; init; }

    /// <summary>
    /// 1: Decimal
    /// 2: Bank Matching
    /// </summary>
    [RequiredIf(nameof(DepositMethod), AutoDepositType.DecimalBankAccount)]
    [PositiveNumber]
    public byte? MatchingMode { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }
}