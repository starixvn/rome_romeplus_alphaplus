namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class AccountOptionDto
{
    public int BankId { get; set; }
    public string BankAccountNumber { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
}