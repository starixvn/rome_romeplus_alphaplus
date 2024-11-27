using System.Text;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Customer;
using Newtonsoft.Json;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.Customer;

public interface ICustomerClient
{
    Task<IEnumerable<UsernameSuggestionViewModel>> GetUsernamesSuggestion(int actorId, string query, bool isInternal, bool directDownlines, bool isAccInfo);

    Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname);
}

public class CustomerClient(
    HttpClient httpClient,
    Fanex.Logging.ILogger logger) : ICustomerClient
{
    private readonly HttpClient httpClient = httpClient;
    private readonly Fanex.Logging.ILogger logger = logger;

    public Task<IEnumerable<UsernameSuggestionViewModel>> GetUsernamesSuggestion(int actorId, string query, bool isInternal, bool directDownlines, bool isAccInfo)
        => PostAsString<IEnumerable<UsernameSuggestionViewModel>>("customer/username-suggestion", new { actorId, query, isInternal, directDownlines, isAccInfo });

    public Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
        => PutAsString<bool>("customer/username-suggestion", new { userId, search, firstName, lastName, nickname });

    public async Task<T> PostAsString<T>(string url, object payload)
    {
        try
        {
            using var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = stringContent };
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
        }

        return default;
    }

    public async Task<T> PutAsString<T>(string url, object payload)
    {
        try
        {
            using var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Put, url) { Content = stringContent };
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
        }

        return default;
    }
}