using Alpha.Tickets.Infrastructure.Criteria;
using Alpha.Tickets.Infrastructure.Models;
using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Repositories;

public interface IAutoFollowBetRepository
{
    Task<int> GetRealCustId(int agentId, int dummyCustId);
}

public class AutoFollowBetRepository : IAutoFollowBetRepository
{
    private readonly IDynamicRepository repository;

    public AutoFollowBetRepository(IDynamicRepository repository)
        => this.repository = repository;

    public async Task<int> GetRealCustId(int agentId, int dummyCustId)
    {
        var followBetAccounts = await repository.FetchAsync<FollowBetAccount>(new GetFollowBetAccountsCriteria(agentId));
        var followBetAccount = followBetAccounts.FirstOrDefault(account => account.DummyCustId == dummyCustId);

        return followBetAccount == null ? 0 : followBetAccount.CustId;
    }
}
