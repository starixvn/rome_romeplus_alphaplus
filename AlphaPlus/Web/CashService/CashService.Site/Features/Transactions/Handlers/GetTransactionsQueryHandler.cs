namespace AlphaPlus.CashService.Site.Features.Transactions.Handlers;

using Helpers;
using MediatR;
using Models;
using Queries;

public class GetTransactionsQueryHandler(HttpClientHelper httpClientHelper)
    : IRequestHandler<GetTransactionsQuery, TransactionResponse>
{
    public async Task<TransactionResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var queryParams = ObjectConverter.ToDictionary(request);
        var result = await httpClientHelper.GetAsync<TransactionResponse>("/transactions/history", queryParams);
        return result;
    }
}
