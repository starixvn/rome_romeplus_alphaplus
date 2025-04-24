using System.Text.Json;

namespace AlphaPlus.CashService.Site.Helpers;

public class HttpClientHelper
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;

    public HttpClientHelper(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.httpClient.BaseAddress = new Uri(configuration.GetValue<string>("CashServiceAPIUrl"));
        jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<T> GetAsync<T>(string path, Dictionary<string, string> queryParams = null)
    {
        var url = BuildUrlWithQueryParams(path, queryParams);
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(jsonOptions);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest data)
    {
        var response = await httpClient.PostAsJsonAsync(path, data, jsonOptions);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(jsonOptions);
    }

    public async Task PutAsync<TRequest>(string path, TRequest data)
    {
        var response = await httpClient.PutAsJsonAsync(path, data, jsonOptions);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string path, Dictionary<string, string> queryParams = null)
    {
        var url = BuildUrlWithQueryParams(path, queryParams);
        var response = await httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private static string BuildUrlWithQueryParams(string path, Dictionary<string, string> queryParams)
    {
        if (queryParams == null || !queryParams.Any())
            return path;

        var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{path}?{query}";
    }
}