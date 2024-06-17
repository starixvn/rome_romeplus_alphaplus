
using AlphaPlus.BetLists.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class LastBetsNewQuery : BetListBaseQuery, IRequest<LastBetsNewViewModel>
{
    public LastBetsNewQuery() : base()
        => UnsavedFilter = Viewer.GetUnsavedFilter(ViewerId, SubAccId);

    public string FilterName { get; set; }

    public int LimitRowCount { get; set; } = 100;

    public bool AutoStart { get; set; } = true;

    public AddFilterParam UnsavedFilter { get; }
}
