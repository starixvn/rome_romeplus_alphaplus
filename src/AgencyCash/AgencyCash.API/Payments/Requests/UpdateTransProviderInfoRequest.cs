using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Payments.Constants;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class UpdateTransProviderInfoRequest
{
    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    [Range(TransactionType.Deposit, TransactionType.Withdrawal, ErrorMessage = "{0} must be {1} (Deposit) or {2} (Withdrawal).")]
    public byte TransType { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(maximumLength: 100, ErrorMessage = "{0} cannot exceed {1} characters.")]
    public string ProviderTransId { get; init; }

    public string IP { get; init; }
}