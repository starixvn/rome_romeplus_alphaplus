using System.Net;
using AlphaPlus.PartialUM.Exceptions;
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

    private static Exception BuildException(ApiError apiError)
        => apiError.Code == PartialUMErrorCode
        ? BuildPartialUMException(apiError)
        : throw new Exception(apiError.Message);

    private static Exception BuildPartialUMException(ApiError apiError)
        => new PartialUMException(
            apiError.Data["startTime"].ToString(),
            apiError.Data["endTime"].ToString());
}