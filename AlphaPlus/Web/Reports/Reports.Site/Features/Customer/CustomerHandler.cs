using AlphaPlus.Reports.Features.Customer;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Customer;

public class CustomerHandler(ICustomerClient customerClient)
    : IRequestHandler<UsernameSuggestionQuery, IEnumerable<UsernameSuggestion>>
{
    public Task<IEnumerable<UsernameSuggestion>> Handle(UsernameSuggestionQuery message, CancellationToken cancellationToken)
        => customerClient.GetUsernamesSuggestion(
            message.ViewerId,
            message.Query,
            message.IsInternal,
            message.DirectDownline,
            message.IsAccInfo);
}