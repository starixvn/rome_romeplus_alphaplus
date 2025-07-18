namespace AlphaPlus.CashService.API.Features.Shared.Helpers;

using System.Text.Json;
using Constants;
using Models;
using Newtonsoft.Json;

public class HttpClientHelper(IHttpClientFactory httpClientFactory)
{
    public async Task<TResponse> PostAsync<TRequest, TResponse>(string baseUrl, string path, TRequest data)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClientName.ProxyHttpClient);
        httpClient.BaseAddress = new Uri(baseUrl);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

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
}