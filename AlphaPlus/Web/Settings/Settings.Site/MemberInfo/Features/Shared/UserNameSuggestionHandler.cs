namespace AlphaPlus.Settings.Site.Features.Shared
{
    using AlphaPlus.Settings.Site.Features.Shared.Requests;
    using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
    using MediatR;
    using System.Collections.Generic;

    public class UserNameSuggestionHandler :
        IRequestHandler<UserNameSuggestionQuery, IEnumerable<UsernameSuggestion>>
    {
        private readonly UsernameSuggestionService usernameSuggestionService;

        public UserNameSuggestionHandler(UsernameSuggestionService usernameSuggestionService)
        {
            this.usernameSuggestionService = usernameSuggestionService;
        }

        public async Task<IEnumerable<UsernameSuggestion>> Handle(UserNameSuggestionQuery message, CancellationToken cancellation)
            => usernameSuggestionService.GetUsernamesSuggestion(
                message.CustId,
                message.SuggestionQuery,
                message.IsInternal,
                message.DirectDownlines,
                message.IsAccInfo);
    }
}