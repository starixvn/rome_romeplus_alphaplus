using AlphaPlus.CashService.Site.Features.Shared.Models;
using MediatR;

namespace AlphaPlus.CashService.Site.Features.Transactions.Commands;

public record RevertTransactionCommand(int TransactionId, string Remarks) : IRequest<ResultModel>;