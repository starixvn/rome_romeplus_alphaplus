namespace AlphaPlus.CashService.API.Extensions;

using System.Text.Json;

public class PaymentGatewayHttpClient
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;

    public PaymentGatewayHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.httpClient.BaseAddress = new Uri(configuration.GetValue<string>("PaymentGatewayConfig:BaseUrl"));
        jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}