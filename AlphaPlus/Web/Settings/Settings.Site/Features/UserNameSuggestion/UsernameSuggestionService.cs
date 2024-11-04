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

        Task<IEnumerable<UsernameSuggestion>> GetUsernameSuggestionByRole(int custId, string username, byte roleId);
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
                [
                    KeyValuePair.Create("actorId", actorId.ToString()),
                    KeyValuePair.Create("query", suggestionQuery),
                    KeyValuePair.Create("isInternal", isInternal.ToString().ToLowerInvariant()),
                    KeyValuePair.Create("directDownlines", directDownline.ToString().ToLowerInvariant()),
                    KeyValuePair.Create("isAccInfo", isAccInfo.ToString().ToLowerInvariant())
                ]
            );

            var result = await httpClient.GetAsJsonAsync<IEnumerable<UsernameSuggestion>>($"customer/username-suggestion{query}");

            return result;
        }

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernameSuggestionByRole(int custId, string username, byte roleId)
        {
            var query = QueryString.Create
            (
                [
                    KeyValuePair.Create("custId", custId.ToString()),
                    KeyValuePair.Create("username", username),
                    KeyValuePair.Create("roleId", roleId.ToString())
                ]
            );

            var result = await httpClient.GetAsJsonAsync<IEnumerable<UsernameSuggestion>>($"customer/username-suggestion-by-role{query}");

            return result;
        }
    }
}