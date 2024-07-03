using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.Site.Features.UserNameSuggestion.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion
{
    public class UserNameSuggestionHandler :
        IRequestHandler<UserNameSuggestionQuery, IEnumerable<UsernameSuggestion>>
    {
        private readonly UsernameSuggestionService usernameSuggestionService;

        public UserNameSuggestionHandler(UsernameSuggestionService usernameSuggestionService)
        {
            this.usernameSuggestionService = usernameSuggestionService;
        }

        public Task<IEnumerable<UsernameSuggestion>> Handle(UserNameSuggestionQuery message, CancellationToken cancellation)
            => usernameSuggestionService.GetUsernamesSuggestion(
                message.CustId,
                message.Query,
                message.IsInternal,
                message.DirectDownlines,
                message.IsAccInfo);
    }
}