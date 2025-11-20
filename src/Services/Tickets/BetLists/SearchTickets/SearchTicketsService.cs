using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.SearchTickets;

public interface ISearchTicketsService
{
    Task<BetListData> GetBetList(SearchTicketsParam param);
}

public class SearchTicketsService(
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService,
    IPartialUMService partialUMService,
    IReportDateService reportDateService) : BetListServiceBase(repository, ipInfoService, ipService), ISearchTicketsService
{
    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly IReportDateService reportDateService = reportDateService;

    public async Task<BetListData> GetBetList(SearchTicketsParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new SearchTicketsCriteria(param.RefNo, param.UplineId, param.UplineLevelId, reportDate.MoveBetsDay, reportDate.BalanceUpDay);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await GetBetList(criteria, param.DisplayVNIP, param.SuperId);
    }
}