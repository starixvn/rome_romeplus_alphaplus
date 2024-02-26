using AlphaPlus.Reports.Features.Shared.Criteria;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Shared;

public interface IAutoFollowBetService
{
    Task<IEnumerable<Tuple<int, IEnumerable<FollowBetAccount>>>> GetFollowBetAccounts(int agentId);
}

public class AutoFollowBetService(IDynamicRepository repository) : IAutoFollowBetService
{
    public async Task<IEnumerable<Tuple<int, IEnumerable<FollowBetAccount>>>> GetFollowBetAccounts(int agentId)
    {
        var followBetAccounts = await repository.FetchAsync<FollowBetAccount>(new GetFollowBetAccountsCriteria(agentId));

        return followBetAccounts
            .GroupBy(f => f.AgentId, (key, accounts) => Tuple.Create(key, accounts));
    }
}
