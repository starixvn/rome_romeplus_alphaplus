using AlphaPlus.CashService.Site.Features.Transactions.Models;
using MediatR;

namespace AlphaPlus.CashService.Site.Features.Transactions.Queries;

public record GetTransactionsQuery : IRequest<TransactionResponse>
{
    public string Username { get; init; }
    public string ReferenceNumber { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}