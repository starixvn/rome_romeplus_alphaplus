namespace AlphaPlus.CashService.Site.Features.Transactions.Requests;

public class RevertTransactionRequest
{
    public int TransactionId { get; set; }
    public string Remarks { get; set; } = string.Empty;
}