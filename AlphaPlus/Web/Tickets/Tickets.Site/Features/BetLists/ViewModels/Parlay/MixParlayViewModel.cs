using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;

public class MixParlayViewModel : BetListBaseViewModel
{
    public MixParlayViewModel(
        BetListBaseQuery message,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper) : base(message, betListData, builder, ticketHelper, supportRiskAmount: true)
        => TicketHelper.IsShowOddsWithChoice = true;
}
