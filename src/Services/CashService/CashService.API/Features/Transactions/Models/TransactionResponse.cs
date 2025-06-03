namespace AlphaPlus.CashService.API.Features.Transactions.Models;

public record TransactionResponse
{
    public IEnumerable<TransactionModel> Items { get; init; } = [];
    public int TotalCount { get; init; }
}