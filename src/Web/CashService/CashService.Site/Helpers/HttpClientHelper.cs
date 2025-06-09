namespace AlphaPlus.CashService.Site.Helpers;

using System.Net;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

public class HttpClientHelper
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;

    public HttpClientHelper(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.httpClient.BaseAddress = new Uri(configuration.GetValue<string>("CashService:APIUrl"));
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
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>(jsonOptions);
        }

        throw new CustomHttpClientException(
            requestUri: new Uri(url),
            requestContent: JsonConvert.SerializeObject(queryParams),
            statusCode: response.StatusCode,
            reasonPhrase: response.ReasonPhrase
        );
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest data)
    {
        var response = await httpClient.PostAsJsonAsync(path, data, jsonOptions);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>(jsonOptions);
        }

        throw new CustomHttpClientException(
            requestUri: new Uri(path),
            requestContent: JsonConvert.SerializeObject(data),
            statusCode: response.StatusCode,
            reasonPhrase: response.ReasonPhrase
        );
    }

    public async Task PutAsync<TRequest>(string path, TRequest data)
    {
        var response = await httpClient.PutAsJsonAsync(path, data, jsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            throw new CustomHttpClientException(
                requestUri: new Uri(path),
                requestContent: JsonConvert.SerializeObject(data),
                statusCode: response.StatusCode,
                reasonPhrase: response.ReasonPhrase
            );
        }
    }

    public async Task DeleteAsync(string path, Dictionary<string, string> queryParams = null)
    {
        var url = BuildUrlWithQueryParams(path, queryParams);
        var response = await httpClient.DeleteAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new CustomHttpClientException(
            requestUri: new Uri(path),
                requestContent: JsonConvert.SerializeObject(queryParams),
                statusCode: response.StatusCode,
                reasonPhrase: response.ReasonPhrase
            );
        }
    }

    private static string BuildUrlWithQueryParams(string path, Dictionary<string, string> queryParams)
    {
        if (queryParams == null || queryParams.Count == 0)
            return $"/api{path}";

        var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        return $"/api{path}?{query}";
    }
}

public class CustomHttpClientException : HttpRequestException
{
    public Uri RequestUri { get; }
    public string RequestContent { get; }

    public CustomHttpClientException(string message, Exception innerException, Uri requestUri, string requestContent, HttpStatusCode statusCode)
        : base(message, innerException, statusCode)
    {
        RequestUri = requestUri;
        RequestContent = requestContent;
    }

    public CustomHttpClientException(Uri requestUri, string requestContent, HttpStatusCode statusCode, string reasonPhrase)
        : base($"Response status code does not indicate success: {(int)statusCode} ({reasonPhrase ?? "Unknown reason"}).", null, statusCode)
    {
        RequestUri = requestUri;
        RequestContent = requestContent;
    }

    public override string Message
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine(base.Message);
            if (RequestUri != null)
            {
                sb.AppendLine($"Request URL: {RequestUri}");
            }
            if (!string.IsNullOrEmpty(RequestContent))
            {
                sb.AppendLine($"Request Data: {RequestContent}");
            }
            return sb.ToString();
        }
    }
}