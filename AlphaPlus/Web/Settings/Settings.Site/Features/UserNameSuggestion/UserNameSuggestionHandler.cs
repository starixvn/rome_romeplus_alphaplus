using AlphaPlus.Settings.Site.Features.UserNameSuggestion.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion
{
    public class UserNameSuggestionHandler(UsernameSuggestionService usernameSuggestionService) :
        IRequestHandler<UserNameSuggestionQuery, IEnumerable<UsernameSuggestion>>
    {
        private readonly UsernameSuggestionService usernameSuggestionService = usernameSuggestionService;

        public async Task<IEnumerable<UsernameSuggestion>> Handle(UserNameSuggestionQuery message, CancellationToken cancellation)
            => await usernameSuggestionService.GetUsernamesSuggestion(
                message.CustId,
                message.Query,
                message.IsInternal,
                message.DirectDownlines,
                message.IsAccInfo);

        public Task<IEnumerable<UsernameSuggestion>> Handle(UserNameSuggestionByRoleQuery message)
            => usernameSuggestionService.GetUsernameSuggestionByRole(message.CustId, message.SuggestionQuery, message.RoleId);
    }
}