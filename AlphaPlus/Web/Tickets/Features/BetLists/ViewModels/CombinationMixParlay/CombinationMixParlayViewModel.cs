using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CombinationMixParlay;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CombinationMixParlay;

public class CombinationMixParlayViewModel : BetListByBetTypeViewModel
{
    public CombinationMixParlayViewModel(
        CombinationMixParlayQuery message,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper,
        IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(message, betListData, builder, ticketHelper, supportScoreMapBetTypeIds)
        => Title = Resources.parlay;
}