using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IAutoFollowBetService
{
    Task<int> GetRealCustId(int agentId, int dummyCustId);
}

public class AutoFollowBetService(IDynamicRepository repository) : IAutoFollowBetService
{
    private readonly IDynamicRepository repository = repository;

    public async Task<int> GetRealCustId(int agentId, int dummyCustId)
    {
        var followBetAccounts = await repository.FetchAsync<FollowBetAccount>(new GetFollowBetAccountsCriteria(agentId));
        var followBetAccount = followBetAccounts.FirstOrDefault(account => account.DummyCustId == dummyCustId);

        return followBetAccount == null ? 0 : followBetAccount.CustId;
    }
}