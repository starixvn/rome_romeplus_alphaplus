using AlphaPlus.Reports.Features.Customer;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Customer;

public class CustomerHandler(ICustomerService customerService)
    : IRequestHandler<UsernameSuggestionQuery, IEnumerable<UsernameSuggestion>>
{
    public Task<IEnumerable<UsernameSuggestion>> Handle(UsernameSuggestionQuery message, CancellationToken cancellationToken)
        => customerService.GetUsernamesSuggestion(new UsernameSuggestionParam(
            message.ViewerId,
            message.CustName,
            message.IsInternal,
            message.DirectDownline,
            message.IsAccInfo));
}