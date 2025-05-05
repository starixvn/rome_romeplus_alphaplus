using AlphaPlus.CashService.Site.Features.Transactions.Models;
using AlphaPlus.CashService.Site.Features.Transactions.Queries;
using MediatR;

namespace AlphaPlus.CashService.Site.Features.Transactions.Handlers;

using Helpers;

public class GetFilterAccountQueryHandler(HttpClientHelper httpClientHelper,
    ILogger<GetFilterAccountQueryHandler> logger
    ) : IRequestHandler<GetFilterAccountQuery, IEnumerable<AccountOptionDto>>
{
    public async Task<IEnumerable<AccountOptionDto>> Handle(GetFilterAccountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var accounts = await httpClientHelper.GetAsync<IEnumerable<AccountOptionDto>>("api/transactions/get-account");
            return accounts ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching filter accounts from {Url}", "getAccount");
            return [];
        }
    }
}