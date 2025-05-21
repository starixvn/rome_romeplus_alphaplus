namespace AlphaPlus.CashService.API.Models.Transactions;

public record TransactionResponse
{
    public IEnumerable<TransactionModel> Items { get; init; } = [];
    public int TotalCount { get; init; }
}