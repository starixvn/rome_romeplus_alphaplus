using AlphaPlus.Tickets.Site.Features.BetLists.Requests.Customer;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Customer;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.Customer;

public class CustomerHandler(ICustomerClient customerClient)
    : IRequestHandler<UsernameSuggestionQuery, IEnumerable<UsernameSuggestionViewModel>>
{
    private readonly ICustomerClient customerClient = customerClient;

    public Task<IEnumerable<UsernameSuggestionViewModel>> Handle(UsernameSuggestionQuery message, CancellationToken cancellationToken)
        => customerClient.GetUsernamesSuggestion(
            message.ViewerId,
            message.Query,
            message.IsInternal,
            // Pass true as DirectDownlines to get all members including their nicknames
            true,
            message.IsAccInfo);
}