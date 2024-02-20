using Alpha.IPInfo.Services;
using AlphaPlus.BetLists.Criteria.RunningBetListByBetTypeAndMatch;
using AlphaPlus.BetLists.Models;
using AlphaPlus.BetLists.Params;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Services;

public interface IRunningBetListByBetTypeAndMatchService
{
    Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param);
}

public class RunningBetListByBetTypeAndMatchService : BetListServiceBase, IRunningBetListByBetTypeAndMatchService
{
    public RunningBetListByBetTypeAndMatchService(
        IDynamicRepository repository,
        IIPInfoService ipInfoService,
        IPService ipService) : base(repository, ipInfoService, ipService)
    {
    }

    public async Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param)
    {
        var (tickets, ticketData) = await repository.FetchMultipleAsync<TicketDto, TicketData>(new GetBetListRunningByBetTypeAndMatchCriteria(
            param.MatchId,
            param.CustId,
            param.BetType,
            param.LiveIndicator,
            param.BetTeam,
            param.CustLevel));
        await AssignCountry(tickets, param.DisplayVNIP);

        return new BetListData(tickets, ticketData);
    }
}
