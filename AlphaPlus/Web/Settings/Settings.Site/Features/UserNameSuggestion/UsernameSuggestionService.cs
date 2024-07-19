using AlphaPlus.Settings.Site.Features.UserNameSuggestion.Models;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion
{
    public interface UsernameSuggestionService
    {
        Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo);
    }

    public class UsernameSuggestionServiceImpl(HttpClient httpClient) : UsernameSuggestionService
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo)
        {
            var query = QueryString.Create
            (
                new[] 
                {
                    KeyValuePair.Create("actorId", actorId.ToString()),
                    KeyValuePair.Create("query", suggestionQuery),
                    KeyValuePair.Create("isInternal", isInternal.ToString().ToLowerInvariant()),
                    KeyValuePair.Create("directDownlines", directDownline.ToString().ToLowerInvariant()),
                    KeyValuePair.Create("isAccInfo", isAccInfo.ToString().ToLowerInvariant())
                }
            );

            var result = await httpClient.GetAsJsonAsync<IEnumerable<UsernameSuggestion>>($"customer/username-suggestion{query}");

            return result;
        }
    }
}