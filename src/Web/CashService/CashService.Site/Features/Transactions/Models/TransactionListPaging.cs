namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class TransactionListPaging
{
    public IEnumerable<Transaction> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}