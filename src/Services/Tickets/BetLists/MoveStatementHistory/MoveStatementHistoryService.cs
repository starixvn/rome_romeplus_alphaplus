using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.MoveStatementHistory;

public interface IMoveStatementHistoryService
{
    Task<IEnumerable<TicketDto>> GetBetList(MoveStatementHistoryParam param);
}

public class MoveStatementHistoryService(
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IMoveStatementHistoryService
{
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<IEnumerable<TicketDto>> GetBetList(MoveStatementHistoryParam param)
    {
        var criteria = new GetBetListMoveStatementHistoryCriteria(param.CustId, param.TransId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var tickets = await repository.FetchAsync<TicketDto>(criteria);
        await AssignCountry(tickets, param.DisplayVNIP);
        await HideIP(tickets, param.SuperId, param.System);

        return tickets;
    }
}