using AlphaPlus.CashService.API.Features.Shared.Requests;

namespace AlphaPlus.CashService.API.Features.Payments.Requests;

public class DeleteBankAccountRequest : BaseActionRequest
{
    public int BankAccountId { get; set; }
} 