using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Payments.Constants;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class AutoDepositRequest
{
    [Range(1, 2, ErrorMessage = "{0} must be {1} (Payment Gateway) or {2} (Decimal or Bank Account).")]
    public byte DepositMethod { get; init; }

    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    [RequiredIf(nameof(DepositMethod), DepositMethodConst.AutoPaymentGateway)]
    [PositiveNumber]
    public short? ProviderId { get; init; }

    [PositiveNumber]
    public short PaymentMethodId { get; init; }

    [RequiredIf(nameof(DepositMethod), DepositMethodConst.AutoDecimalOrBankAccount)]
    [PositiveNumber]
    public byte? MachingMode { get; init; }

    [RequiredIf(nameof(DepositMethod), DepositMethodConst.AutoDecimalOrBankAccount)]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentFrom { get; init; }

    [RequiredIf(nameof(DepositMethod), DepositMethodConst.AutoDecimalOrBankAccount)]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string PaymentTo { get; init; }

    [PositiveNumber]
    public decimal Amount { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }
}