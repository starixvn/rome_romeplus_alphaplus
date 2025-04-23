using System.Text.Json;

namespace AlphaPlus.CashService.Site.Helpers;
public class HttpClientHelper
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpClientHelper(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("CashServiceAPIUrl") ?? "http://localhost:5252");
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<T> GetAsync<T>(string path, Dictionary<string, string> queryParams = null)
    {
        var url = BuildUrlWithQueryParams(path, queryParams);
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest data)
    {
        var response = await _httpClient.PostAsJsonAsync(path, data, _jsonOptions);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
    }

    public async Task PutAsync<TRequest>(string path, TRequest data)
    {
        var response = await _httpClient.PutAsJsonAsync(path, data, _jsonOptions);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string path, Dictionary<string, string> queryParams = null)
    {
        var url = BuildUrlWithQueryParams(path, queryParams);
        var response = await _httpClient.DeleteAsync(url);
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