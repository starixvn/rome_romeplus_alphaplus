using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.RunningBetListByBetType;

public interface IRunningBetListByBetTypeService
{
    Task<IEnumerable<TicketDto>> GetBetList(RunningBetListByBetTypeParam param);
}

public class RunningBetListByBetTypeService(
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IRunningBetListByBetTypeService
{
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<IEnumerable<TicketDto>> GetBetList(RunningBetListByBetTypeParam param)
    {
        var criteria = new GetBetListRunningByBetTypeCriteria(param.CustId, param.BetType, param.CustLevelId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var tickets = await repository.FetchAsync<TicketDto>(criteria);
        var shouldHideIP = await ipService.CheckHideIP(param.SuperId, param.System);
        AssignIP(tickets, shouldHideIP);
        await AssignCountry(tickets, param.DisplayVNIP, shouldHideIP);
        await UpdateOddsMixParlay(tickets);

        return tickets;
    }
}