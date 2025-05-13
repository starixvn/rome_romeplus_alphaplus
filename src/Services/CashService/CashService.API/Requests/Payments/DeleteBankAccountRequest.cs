namespace AlphaPlus.CashService.API.Requests.Payments;

public class DeleteBankAccountRequest : BaseActionRequest
{
    public int BankAccountId { get; set; }
} 