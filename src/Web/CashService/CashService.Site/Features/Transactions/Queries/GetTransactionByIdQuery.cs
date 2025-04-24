using AlphaPlus.CashService.Site.Features.Transactions.Models;
using MediatR;

namespace AlphaPlus.CashService.Site.Features.Transactions.Queries;

public record GetTransactionByIdQuery(int TransactionId) : IRequest<TransactionModel>;
