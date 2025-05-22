
using AlphaPlus.CashService.Site.Features.Shared.Requests;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests;

public class DeleteBankAccountRequest : BaseActionRequest
{
    public int BankAccountId { get; set; }
} 