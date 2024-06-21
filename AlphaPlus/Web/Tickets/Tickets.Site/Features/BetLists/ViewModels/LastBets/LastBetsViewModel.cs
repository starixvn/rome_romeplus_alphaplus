using AlphaPlus.BetLists.AppResources;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;

public class LastBetsViewModel(
    LastBetsQuery message,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    string lastTransIdCacheKey,
    string latestTransIdCacheKey)
        : BetListBaseViewModel(message, null, builder, ticketHelper, showMemberName: true)
{
    public int BetType { get; } = message.BetType;

    public int PageSize { get; } = message.PageSize;

    public string LastTransIdCacheKey { get; } = lastTransIdCacheKey;

    public string LatestTransIdCacheKey { get; } = latestTransIdCacheKey;

    public bool ResetLastTransId { get; } = message.BetType == BetTypeConstants.AllBetTypes;

    public int[] PageSizes { get; } = [10, 20, 30];

    public int[] AutoRefreshSeconds { get; } = [30, 60, 90];

    protected override string GetTitle() => Resources.LastBets;
}
