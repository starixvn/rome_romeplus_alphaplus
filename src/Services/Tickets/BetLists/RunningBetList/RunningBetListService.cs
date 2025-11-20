using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.RunningBetList;

public interface IRunningBetListService
{
    Task<BetListData> GetBetList(RunningBetListParam param);
}

public class RunningBetListService(
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IRunningBetListService
{
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<BetListData> GetBetList(RunningBetListParam param)
    {
        var criteria = new GetBetListRunningByDateCriteria(
            param.CustId,
            param.ProductIds,
            param.FromDate,
            param.ToDate,
            param.GetAll,
            param.UserName);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var betListData = await GetBetList(criteria, param.DisplayVNIP, param.SuperId);
        await UpdateOddsMixParlay(betListData.Tickets);

        return betListData;
    }
}
