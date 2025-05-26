namespace AlphaPlus.CashService.API.Features.Transactions.Models;

public class TransactionLog
{
    public int UserId { get; set; }
    public int FromCustId { get; set; }
    public int ToCustId { get; set; }
    public string IP { get; set; }
    public string Doing { get; set; }
}