using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.ParlayWinLossByMatch;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.ParlayWinLossByMatch;

public class ParlayWinLossByMatchQuery : BetListBaseQuery, IRequest<ParlayWinLossByMatchViewModel>
{
    public int BetType { get; set; }

    public byte CustLevelId { get; set; }

    public string LangKey => Viewer.LangKey;

    public DateTime LimitedDateToShowInfo => Viewer.LimitedDateToShowInfo;
}
