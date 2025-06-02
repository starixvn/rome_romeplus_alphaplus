using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AlphaPlus.PaymentService.API.Shared;

public abstract class PaymentServiceBase(HttpClient client, Fanex.Logging.ILogger logger)
{
    private static readonly JsonSerializerOptions defaultJsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null,
        WriteIndented = false
    };
    protected readonly HttpClient client = client;
    protected readonly Fanex.Logging.ILogger logger = logger;

    protected async Task<T> PostAsync<T>(string url, object payload, string mediaType = "application/json")
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(payload, defaultJsonSerializerOptions);
            using var jsonPayload = new StringContent(jsonString, Encoding.UTF8, mediaType);
            using var response = await client.PostAsync(url, jsonPayload).ConfigureAwait(false);
            var jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(jsonContent, defaultJsonSerializerOptions);
            }

            await logger.CriticalAsync($"Call {url} failed with status code {response.StatusCode}. Response: {jsonContent}. Request payload: {jsonString}");
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"Exception calling {url}: {ex.Message}", ex);
        }

        return default;
    }
}