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
            bool isAccInfo,
            bool excludeMember,
            bool excludeCashMember);

        Task<IEnumerable<UsernameSuggestion>> GetUsernameSuggestionByRole(int custId, string username, byte roleId);

        Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname);
    }

    public class UsernameSuggestionServiceImpl(HttpClient httpClient) : UsernameSuggestionService
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo,
            bool excludeMember,
            bool excludeCashMember)
        {
            var result = await httpClient.PostAsJsonAsync<IEnumerable<UsernameSuggestion>>(
                $"customer/username-suggestion",
                new
                {
                    actorId,
                    query = suggestionQuery,
                    isInternal,
                    directDownlines = directDownline,
                    isAccInfo,
                    excludeMember,
                    excludeCashMember
                });

            return result;
        }

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernameSuggestionByRole(int custId, string username, byte roleId)
        {
            var result = await httpClient.PostAsJsonAsync<IEnumerable<UsernameSuggestion>>(
                $"customer/username-suggestion-by-role",
                new
                {
                    custId,
                    username,
                    roleId
                });

            return result;
        }

        public Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
            => httpClient.PutAsStringAsync("customer/username-suggestion", new { userId, search, firstName, lastName, nickname });
    }
}