namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Helpers;

using System.Net;
using System.Text;
using System.Text.Json;
using AlphaPlus.CashService.API.Features.Shared.Models;
using Newtonsoft.Json;
using Shared.Constants;

public class PaymentGatewayHttpClientHelper
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;

    public PaymentGatewayHttpClientHelper(IHttpClientFactory httpClientFactory, PaymentGatewayApiConfig apiConfig)
    {
        httpClient = httpClientFactory.CreateClient(HttpClientName.ProxyHttpClient);
        httpClient.BaseAddress = new Uri(apiConfig.BaseUrl);
        httpClient.DefaultRequestHeaders.Add("MerchantNo", apiConfig.MerchantNo);
        httpClient.DefaultRequestHeaders.Add("AppSecret", apiConfig.AppSecret);
        httpClient.DefaultRequestHeaders.Add("SecretKey", apiConfig.SecretKey);
        jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
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
}