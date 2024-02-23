using System.Net;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared;

public class ServiceBase
{
    private const int PartialUMErrorCode = 1001;
    protected readonly HttpClient client;

    protected ServiceBase(HttpClient client)
        => this.client = client;

    public async Task<T> GetFromJson<T>(string url)
    {
        try
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
            }

            var apiError = await response.Content.ReadFromJsonAsync<ApiError>().ConfigureAwait(false);
            throw BuildException(apiError);
        }
        catch (HttpRequestException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            var abc = ex;
        }

        return default;
    }

    private static Exception BuildException(ApiError apiError)
        => apiError.Code == PartialUMErrorCode
        ? BuildPartialUMException(apiError)
        : throw new Exception(apiError.Message);

    private static Exception BuildPartialUMException(ApiError apiError)
        => new PartialUMException(
            apiError.Data["startTime"].ToString(),
            apiError.Data["endTime"].ToString());
}

[Serializable]
public class PartialUMException(string StartTime, string EndTime) : Exception
{
    public int ErrorCode { get; } = 9999;

    public IDictionary<string, string> ErrorData { get; } = new Dictionary<string, string>
    {
        [nameof(StartTime)] = StartTime,
        [nameof(EndTime)] = EndTime
    };
}