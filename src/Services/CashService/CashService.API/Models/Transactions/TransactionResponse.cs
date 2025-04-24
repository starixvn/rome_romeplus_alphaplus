namespace AlphaPlus.CashService.API.Models.Transactions;

public record TransactionResponse
{
    public IEnumerable<TransactionModel> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}