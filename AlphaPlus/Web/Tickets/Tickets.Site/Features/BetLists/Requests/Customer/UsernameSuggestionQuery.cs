using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Customer;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.Customer;

public class UsernameSuggestionQuery : BetListBaseQuery, IRequest<IEnumerable<UsernameSuggestionViewModel>>
{
    public string Query { get; set; }

    public bool DirectDownline { get; set; }

    public bool IsAccInfo { get; set; } = true;
}
