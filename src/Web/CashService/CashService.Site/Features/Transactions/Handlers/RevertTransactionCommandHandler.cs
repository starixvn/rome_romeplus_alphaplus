namespace AlphaPlus.CashService.Site.Features.Transactions.Handlers;

using AlphaPlus.CashService.Site.Features.Shared.Models;
using Commands;
using Helpers;
using MediatR;

public class RevertTransactionCommandHandler(HttpClientHelper httpClientHelper)
    : IRequestHandler<RevertTransactionCommand, ResultModel>
{
    public async Task<ResultModel> Handle(RevertTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await httpClientHelper.PostAsync<RevertTransactionCommand, ResultModel>("/api/transactions/revert-pending", request);
        return result;
    }
}