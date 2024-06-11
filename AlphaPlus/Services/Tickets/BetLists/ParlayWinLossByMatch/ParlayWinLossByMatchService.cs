using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.ParlayWinLossByMatch;

public interface IParlayWinLossByMatchService
{
    Task<IEnumerable<TicketDto>> GetBetList(ParlayWinLossByMatchParam param);
}

public class ParlayWinLossByMatchService(
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IParlayWinLossByMatchService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<IEnumerable<TicketDto>> GetBetList(ParlayWinLossByMatchParam param)
    {
        var criteria = new GetParlayWinLossByMatchCriteria(
            param.CustId,
            param.BetType,
            param.FromDate,
            param.ToDate,
            await reportDateService.GetMoveBetsDay(),
            param.CustLevel);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var tickets = await repository.FetchAsync<TicketDto>(criteria);
        await AssignCountry(tickets, param.DisplayVNIP);

        return tickets;
    }
}