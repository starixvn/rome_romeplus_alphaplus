using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;

public class LastBetsDataNewViewModel(
    long latestTransId,
    string lastTransIdCacheKeyByFilter,
    string latestTransIdCacheKeyByFilter,
    BetListBaseQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : BetListBaseViewModel(message, betListData, builder, ticketHelper, showMemberName: true)
{
    public long LatestTransId { get; } = latestTransId;

    public string LastTransIdCacheKeyByFilter { get; } = lastTransIdCacheKeyByFilter;

    public string LatestTransIdCacheKeyByFilter { get; } = latestTransIdCacheKeyByFilter;

    public long LastTransId
        => BetListData?.Tickets.Any() == true
        ? BetListData.Tickets.ElementAt(BetListData.Total - 1).TransId
        : 0;
}
