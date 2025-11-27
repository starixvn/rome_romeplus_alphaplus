using AlphaPlus.BetLists.Parlay;
using Fanex.BetList.Core.Builder.HybridParlayBuilder;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.BetBuilderPlus;

public interface IBetBuilderPlusService
{
    Task<string> GetBetBuilderPlusTicketsDetailsHtml(ITicketHelper ticketHelper, BetBuilderPlusParam betBuilderPlusParam);
}

public class BetBuilderPlusService(IDynamicRepository repository) : IBetBuilderPlusService
{
    private readonly IDynamicRepository repository = repository;

    public async Task<string> GetBetBuilderPlusTicketsDetailsHtml(ITicketHelper ticketHelper, BetBuilderPlusParam betBuilderPlusParam)
    {
        var tickets = await repository
            .FetchAsync<Ticket>(new GetParlayBetListCriteria(betBuilderPlusParam.TransId, betBuilderPlusParam.CustId));
        var stakeTicket = tickets.FirstOrDefault(x => x.Stake > 0);
        
        var betBuilderPlusBuilder = new HyBridParlayBuilder();

        return betBuilderPlusBuilder.Build(
            ticketHelper, [.. tickets],
            betBuilderPlusParam.TransDesc,
            (int)betBuilderPlusParam.Hdp1,
            (int)betBuilderPlusParam.Hdp2,
            betBuilderPlusParam.BetCheck,
            betBuilderPlusParam.BetTeam,
            stakeTicket != null ? stakeTicket.Currency : betBuilderPlusParam.Currency,
            stakeTicket != null ? stakeTicket.Stake : betBuilderPlusParam.Stake,
            false);
    }
}