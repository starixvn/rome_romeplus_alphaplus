using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CorrectScore;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CorrectScore;

public class CorrectScoreViewModel(
    CorrectScoreQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    HashSet<int> supportScoreMapBetTypeIds = null)
        : BetListByBetTypeViewModel(message, betListData, builder, ticketHelper, supportScoreMapBetTypeIds, supportRiskAmount: true)
{
}
