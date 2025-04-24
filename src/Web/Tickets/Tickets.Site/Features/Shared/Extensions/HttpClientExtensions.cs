using System.Net;
using AlphaPlus.PartialUM.Exceptions;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T> GetJson<T>(this HttpClient httpClient, string url)
    {
        var response = await httpClient.GetAsync(url).ConfigureAwait(false);
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
        : new Exception(apiError.Message);
}