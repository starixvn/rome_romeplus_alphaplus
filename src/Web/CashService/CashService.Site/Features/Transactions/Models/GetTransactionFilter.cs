namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class GetTransactionFilter
{
    public int TransactionType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int StatusId { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}