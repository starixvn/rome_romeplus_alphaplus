using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.BetBuilder;

public interface IBetBuilderService
{
    Task<IEnumerable<BetBuilderDetailModel>> GetBetList(int custId, long transId);
}

public class BetBuilderService(IDynamicRepository repository) : IBetBuilderService
{
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<BetBuilderDetailModel>> GetBetList(int custId, long transId)
    {
        var criteria = new GetBetBuilderDetailCriteria(custId, transId);

        return repository.FetchAsync<BetBuilderDetailModel>(criteria);
    }
}