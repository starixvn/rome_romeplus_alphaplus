using System.Text;
using System.Text.Json;

namespace AlphaPlus.Core.Authorization;

public static class HttpClientExtensions
{
    public static async Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, string requestUri)
    {
        var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        return await ToObject<T>(response);
    }

    public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, object payload = null)
    {
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(requestUri, content).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        return await ToObject<T>(response);
    }

    private static async Task<T> ToObject<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(content);
    }
}
