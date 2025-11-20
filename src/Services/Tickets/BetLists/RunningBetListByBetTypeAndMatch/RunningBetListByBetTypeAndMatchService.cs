using AlphaPlus.IPInfo.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;

namespace AlphaPlus.BetLists.RunningBetListByBetTypeAndMatch;

public interface IRunningBetListByBetTypeAndMatchService
{
    Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param);
}

public class RunningBetListByBetTypeAndMatchService(
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IRunningBetListByBetTypeAndMatchService
{
    public Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param)
        => GetBetList(new GetBetListRunningByBetTypeAndMatchCriteria(
            param.MatchId,
            param.CustId,
            param.BetType,
            param.LiveIndicator,
            param.BetTeam,
            param.CustLevelId), param.DisplayVNIP, param.SuperId, param.System);
}
