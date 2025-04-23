using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;

public class LastBetsDataViewModel(
    long latestTransId,
    GetLastBetsDataQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    HashSet<int> supportScoreMapBetTypeIds)
        : BetListBaseViewModel(message, betListData, builder, ticketHelper, showMemberName: true)
{
    public long LatestTransId { get; } = latestTransId;

    public long LastTransId
        => BetListData.Tickets.Any()
        ? BetListData.Tickets.ElementAt(BetListData.Total - 1).TransId
        : 0;

    public override bool CanViewScoreMap(ITicket ticket)
    {
        var status = ((TicketDto)ticket).MatchStatus;

        return !string.IsNullOrWhiteSpace(status)
            && !status.Equals("completed", StringComparison.OrdinalIgnoreCase)
            && supportScoreMapBetTypeIds.Contains(ticket.BetTypeId);
    }
}