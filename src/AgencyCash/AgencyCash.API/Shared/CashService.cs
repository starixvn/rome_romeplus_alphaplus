using AgencyCash.API.Shared.Criteria;
using Fanex.Data.Repository;

namespace AgencyCash.API.Shared;

public interface ICashService
{
    Task<bool> IsEnableCashService(int agentId);
}

public class CashService(IDynamicRepository dynamicRepository, ISiteDBService siteDBService) : ICashService
{
    private const byte CashServiceType = 1;

    private readonly IDynamicRepository dynamicRepository = dynamicRepository;
    private readonly ISiteDBService siteDBService = siteDBService;

    public async Task<bool> IsEnableCashService(int agentId)
    {
        var custDBGroupId = await siteDBService.GetCustDBGroupId(agentId);

        var criteria = new GetAgentServiceTypeCriteria(agentId, custDBGroupId);
        await dynamicRepository.ExecuteAsync(criteria);

        return criteria.ServiceType == CashServiceType;
    }
}