namespace AlphaPlus.CashService.Site.Features.Transactions.Handlers;

using Core.Models;
using Helpers;
using MediatR;
using Models;
using Queries;

public class GetFilterAccountQueryHandler(HttpClientHelper httpClientHelper,
    IUserSession userSession,
    ILogger<GetFilterAccountQueryHandler> logger
    ) : IRequestHandler<GetFilterAccountQuery, IEnumerable<AccountOptionDto>>
{
    public async Task<IEnumerable<AccountOptionDto>> Handle(GetFilterAccountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var accounts = await httpClientHelper.GetAsync<IEnumerable<AccountOptionDto>>($"api/common/agent-bank-list?agentId={userSession.CustId}&transactionType={request.TransactionType}");
            return accounts ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching filter accounts from {Url}", "getAccount");
            return [];
        }
    }
}