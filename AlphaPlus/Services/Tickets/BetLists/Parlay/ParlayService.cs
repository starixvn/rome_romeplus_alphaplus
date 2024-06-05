using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Parlay;

public interface IParlayService
{
    Task<IEnumerable<TicketDto>> GetParlayBetList(long transId, int custId);

    Task<ComboParlayDetailData> GetComboParlayDetail(long refNo, int custId, string language);
}

public class ParlayService(IReportDateService reportDateService, IDynamicRepository repository) : IParlayService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<TicketDto>> GetParlayBetList(long transId, int custId)
        => repository.FetchAsync<TicketDto>(new GetParlayBetListCriteria(transId, custId));

    public async Task<ComboParlayDetailData> GetComboParlayDetail(long refNo, int custId, string language)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new GetComboParlayDetailCriteria(
            refNo,
            custId,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay);
        var (comboParlayDetails, matchDetails) = await repository.FetchMultipleAsync<ComboParlayDetail, MatchDetail>(criteria);

        return new ComboParlayDetailData(comboParlayDetails, matchDetails, criteria.Outstanding ?? 0, criteria.IsParlayBoost);
    }
}