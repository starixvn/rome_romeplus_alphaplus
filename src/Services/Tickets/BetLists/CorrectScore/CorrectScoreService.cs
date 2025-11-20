using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CorrectScore;

public interface ICorrectScoreService
{
    Task<IEnumerable<TicketDto>> GetBetList(CorrectScoreParam param);
}

public class CorrectScoreService(
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), ICorrectScoreService
{
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<IEnumerable<TicketDto>> GetBetList(CorrectScoreParam param)
    {
        var criteria = new GetRunningCorrectScoreCriteria(param.CustId, param.CustLevelId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var tickets = await repository.FetchAsync<TicketDto>(criteria);
        await AssignCountry(tickets, param.DisplayVNIP);
        await HideIP(tickets, param.SuperId);

        return tickets;
    }
}