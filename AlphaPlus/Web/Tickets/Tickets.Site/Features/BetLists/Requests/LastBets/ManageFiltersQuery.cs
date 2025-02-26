using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class ManageFiltersQuery : BetListBaseQuery, IRequest<ManageFiltersViewModel>
{
    public string FilterName { get; set; }

    public int LimitRowCount { get; set; }

    public bool AutoStart { get; set; }
}
