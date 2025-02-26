using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.JackpotDetails;

public interface IJackpotDetailsService
{
    Task<IEnumerable<TicketDto>> GetBetList(JackpotDetailsParam param);
}

public class JackpotDetailsService(
    IReportDateService reportDateService,
    IDynamicRepository repository) : IJackpotDetailsService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IDynamicRepository repository = repository;

    public async Task<IEnumerable<TicketDto>> GetBetList(JackpotDetailsParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new GetJackpotDetailsCriteria(param.TransId,
            param.CustId,
            param.BetType,
            reportDate.MoveBetsDay,
            reportDate.BalanceUpDay);

        return await repository.FetchAsync<TicketDto>(criteria);
    }
}
