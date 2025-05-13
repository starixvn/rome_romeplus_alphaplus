
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests;

public class DeleteBankAccountRequest : BaseActionRequest
{
    public int BankAccountId { get; set; }
} 