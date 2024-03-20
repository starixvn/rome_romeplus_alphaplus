using System.Text;
using System.Text.Json;

public static class HttpClientExtensions
{
    private static readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, string requestUri)
    {
        var response = await httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, serializerOptions);
    }

    public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, object payload = null)
    {
        var response = await httpClient.PostAsStringAsync(requestUri, payload);
        return JsonSerializer.Deserialize<T>(response, serializerOptions);
    }

    public static async Task<string> PostAsStringAsync(this HttpClient httpClient, string requestUri, object payload = null)
    {
        using var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        using var response = await httpClient.PostAsync(requestUri, content).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> PutAsStringAsync(this HttpClient httpClient, string requestUri, object value = null)
    {
        var json = value != null ? JsonSerializer.Serialize(value) : string.Empty;
        var contentString = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(httpClient.BaseAddress, requestUri),
            Content = contentString
        };
        var response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<T> PutAsJsonAsync<T>(this HttpClient httpClient, string requestUri, object value = null)
    {
        var content = await PutAsStringAsync(httpClient, requestUri, value);
        return JsonSerializer.Deserialize<T>(content, serializerOptions);
    }
}