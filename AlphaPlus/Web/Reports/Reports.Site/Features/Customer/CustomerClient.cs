using System.Net;
using System.Text;
using AlphaPlus.Reports.Features.Customer;
using Newtonsoft.Json;

namespace AlphaPlus.Reports.Site.Features.Customer;

public interface ICustomerClient
{
    Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
        int viewerId,
        string query,
        bool isInternal,
        bool directDownline,
        bool isAccInfo);

    Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname);
}

public class CustomerClient(HttpClient httpClient) : ICustomerClient
{
    private readonly HttpClient httpClient = httpClient;

    public Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(int viewerId, string query, bool isInternal, bool directDownline, bool isAccInfo)
    {
        var queryString = QueryString.Create([
            KeyValuePair.Create("actorId", viewerId.ToString()),
            KeyValuePair.Create(nameof(query), query),
            KeyValuePair.Create(nameof(isInternal), ToString(isInternal)),
            KeyValuePair.Create("directDownlines", ToString(directDownline)),
            KeyValuePair.Create(nameof(isAccInfo), ToString(isAccInfo))
        ]);

        return GetJson<IEnumerable<UsernameSuggestion>>($"customer/username-suggestion{queryString}");
    }

    public Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
        => PostJson<bool>("customer/username-suggestion", new { userId, search, firstName, lastName, nickname });

    private static string ToString(bool value) => value.ToString().ToLowerInvariant();

    private async Task<T> GetJson<T>(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
        }

        return default;
    }

    public async Task<T> PostJson<T>(string url, object payload)
    {
        try
        {
            using var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
        }

        return default;
    }
}