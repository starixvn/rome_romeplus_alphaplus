using System.Net;
using AlphaPlus.PartialUM.Exceptions;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared;

public class ServiceBase
{
    protected readonly HttpClient client;

    protected ServiceBase(HttpClient client)
        => this.client = client;

    public async Task<T> GetFromJson<T>(string url)
    {
        var response = await client.GetAsync(url).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        }

        var apiError = await response.Content.ReadFromJsonAsync<APIError>().ConfigureAwait(false);
        throw BuildException(apiError);
    }

    private static Exception BuildException(APIError apiError)
        => apiError.Code == PartialUMException.ErrorCode
        ? new PartialUMException(apiError.StartTime, apiError.EndTime)
        : new Exception(apiError.message, CancellationToken cancellation);
}
