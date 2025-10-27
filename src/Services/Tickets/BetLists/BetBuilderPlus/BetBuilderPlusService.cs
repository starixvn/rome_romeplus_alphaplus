using AlphaPlus.BetLists.Parlay;
using Fanex.BetList.Core.Builder.HybridParlayBuilder;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.BetBuilderPlus;

public interface IBetBuilderPlusService
{
    Task<string> GetBetBuilderPlusTicketsDetailsHtml(ITicketHelper ticketHelper, long transId, int custId, string transDesc);
}

public class BetBuilderPlusService(IDynamicRepository repository) : IBetBuilderPlusService
{
    private readonly IDynamicRepository repository = repository;

    public async Task<string> GetBetBuilderPlusTicketsDetailsHtml(ITicketHelper ticketHelper, long transId, int custId, string transDesc)
    {
        var tickets = await repository.FetchAsync<Ticket>(new GetParlayBetListCriteria(transId, custId));
        
        var betBuilderPlusBuilder = new HyBridParlayBuilder();

        return betBuilderPlusBuilder.Build(ticketHelper, [.. tickets], transDesc, false);
    }
}