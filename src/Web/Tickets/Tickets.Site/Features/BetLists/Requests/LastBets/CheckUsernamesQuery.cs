using AlphaPlus.BetLists.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class CheckUsernamesQuery : BetListBaseQuery, IRequest<IEnumerable<UsernameErrCode>>
{
    public string Usernames { get; set; }

    public bool FromSuggestion { get; set; }
}