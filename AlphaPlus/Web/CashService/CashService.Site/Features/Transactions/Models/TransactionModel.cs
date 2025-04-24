namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public record TransactionModel
{
    public int TransactionId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public bool IsDeposit { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Status { get; set; }
    public string TransactionType { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime DateTime { get; set; }
}

public record TransactionResponse
{
    public IEnumerable<TransactionModel> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}