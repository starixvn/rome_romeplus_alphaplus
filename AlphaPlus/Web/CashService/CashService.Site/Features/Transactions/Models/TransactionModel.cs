namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public record TransactionModel
{
    public int Id { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string Description { get; init; } = string.Empty;
}

public record TransactionResponse
{
    public IEnumerable<TransactionModel> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
