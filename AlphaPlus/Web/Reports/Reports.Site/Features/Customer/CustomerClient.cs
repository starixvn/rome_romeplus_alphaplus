using System.Net;
using AlphaPlus.Reports.Features.Customer;

namespace AlphaPlus.Reports.Site.Features.Customer;

public interface ICustomerClient
{
    Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
        int viewerId,
        string query,
        bool isInternal,
        bool directDownline,
        bool isAccInfo);
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
}