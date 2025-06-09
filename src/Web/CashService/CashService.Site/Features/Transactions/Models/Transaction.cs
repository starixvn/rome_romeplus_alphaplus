namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public record TransactionResponse
{
    public IEnumerable<Transaction> Items { get; set; }
}
public record Transaction
{
    public string TransId { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PaymentFrom { get; set; }
    public string PaymentTo { get; set; }
    public int StatusId { get; set; }
    public string TransactionType { get; set; }
    public DateTime CreatedTime { get; set; }
}