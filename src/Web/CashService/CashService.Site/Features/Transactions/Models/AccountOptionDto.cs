namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class AccountOptionDto
{
    public int BankId { get; set; }
    public string BankAccountNumber { get; set; } = string.Empty;
}