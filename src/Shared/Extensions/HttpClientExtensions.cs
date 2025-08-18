using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;

[SuppressMessage("SonarQube", "S3903:Types should be defined in named namespaces", Justification = "This is a static class that contains extension methods for IEnumerable<T>.")]
internal static class HttpClientExtensions
{
    public static async Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, string requestUri)
    {
        var response = await httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content);
    }

    public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, object payload = null)
    {
        var response = await httpClient.PostAsStringAsync(requestUri, payload);
        return JsonConvert.DeserializeObject<T>(response);
    }

    public static async Task<string> PostAsStringAsync(this HttpClient httpClient, string requestUri, object payload = null)
    {
        using var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        using var response = await httpClient.PostAsync(requestUri, content).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> PutAsStringAsync(this HttpClient httpClient, string requestUri, object value = null)
    {
        var json = value != null ? JsonConvert.SerializeObject(value) : string.Empty;
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
        return JsonConvert.DeserializeObject<T>(content);
    }
}