namespace AlphaPlus.Settings.MemberInfo.API.Features.Shared
{
    using AlphaPlus.Settings.MemberInfo.API.Features.Shared.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public interface UsernameSuggestionService
    {
        IEnumerable<UsernameSuggestion> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo);
    }

    public class UsernameSuggestionServiceImpl : UsernameSuggestionService
    {
        private readonly IDynamicRepository dynamicRepository;

        public UsernameSuggestionServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<UsernameSuggestion> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo)
            => dynamicRepository.Fetch<UsernameSuggestion>(
              new SelectAutoSuggestionCriteria(
                  actorId,
                  suggestionQuery,
                  isInternal,
                  directDownline,
                  isAccInfo)).OrderBy(item => item.UserName);
    }
}