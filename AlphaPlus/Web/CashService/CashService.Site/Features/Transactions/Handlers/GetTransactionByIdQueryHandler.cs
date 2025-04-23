namespace AlphaPlus.CashService.Site.Features.Transactions.Handlers;

using Helpers;
using MediatR;
using Models;
using Queries;

public class GetTransactionByIdQueryHandler(HttpClientHelper httpClientHelper)
    : IRequestHandler<GetTransactionByIdQuery, TransactionModel>
{
    public async Task<TransactionModel> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await httpClientHelper.GetAsync<TransactionModel>($"/transactions/history/{request.TransactionId}");
        return result;
    }
}