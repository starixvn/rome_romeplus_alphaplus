using System.Net;
using System.Text;

namespace AgencyCash.APIGateway.DelegatingHandlers;

public sealed class ApiKeyHandler(IConfiguration configuration) : DelegatingHandler
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private readonly IConfiguration configuration = configuration;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.TryGetValues(ApiKeyHeaderName, out var headers))
        {
            var userApiKey = headers.First();

            if (!string.IsNullOrWhiteSpace(userApiKey))
            {
                const string ApiKeyName = "ApiKey";

                return userApiKey == configuration[ApiKeyName]
                    ? base.SendAsync(request, cancellationToken)
                    : CreateErrorResponse(HttpStatusCode.Unauthorized, $"{ApiKeyHeaderName} is invalid.");
            }
        }

        return CreateErrorResponse(HttpStatusCode.BadRequest, $"{ApiKeyHeaderName} is required.");
    }

    private static Task<HttpResponseMessage> CreateErrorResponse(HttpStatusCode statusCode, string error)
        => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(error, Encoding.UTF8, "application/json")
        });
}
