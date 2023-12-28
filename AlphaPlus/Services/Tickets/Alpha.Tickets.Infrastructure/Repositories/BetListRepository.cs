using Alpha.Tickets.Infrastructure.Criteria.BetList;
using Alpha.Tickets.Infrastructure.Models.BetList;
using Alpha.Tickets.Infrastructure.Params.BetList;
using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Repositories;

public interface IBetListRepository
{
    Task<BetListData> GetBetListRunningByDate(RunningBetListParam param);

    Task<BetListData> GetBetListAllByDate(RunningBetListParam param);

    Task<IEnumerable<OddsMixParlay>> GetOddsMixParlay(string transIds);
}

public class BetListRepository : IBetListRepository
{
    private readonly IDynamicRepository repository;

    public BetListRepository(IDynamicRepository repository)
        => this.repository = repository;

    public async Task<BetListData> GetBetListAllByDate(RunningBetListParam param)
    {
        var (tickets, ticketData) = await repository.FetchMultipleAsync<Ticket, TicketData>(
            new GetBetListAllByDateCriteria(
                param.CustId,
                param.ProductIds,
                param.FromDate,
                param.ToDate,
                param.Username));

        return new BetListData(tickets, ticketData);
    }

    public async Task<BetListData> GetBetListRunningByDate(RunningBetListParam param)
    {
        var (tickets, ticketData) = await repository.FetchMultipleAsync<Ticket, TicketData>(
            new GetBetListRunningByDateCriteria(
                param.CustId,
                param.ProductIds,
                param.FromDate,
                param.ToDate,
                param.GetAll,
                param.Username));

        return new BetListData(tickets, ticketData);
    }

    public Task<IEnumerable<OddsMixParlay>> GetOddsMixParlay(string transIds)
        => repository.FetchAsync<OddsMixParlay>(new GetOddsOfMixParlayCriteria(transIds));
}
