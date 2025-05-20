namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class GetTransactionFilter
{
    public int CustId { get; set; } 

    public int TransactionType { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int PaymentStatus { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}