using System.Net.Http.Headers;

namespace AgencyCash.APIGateway.DelegatingHandlers;

public sealed class NoGzipDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.AcceptEncoding.Clear();
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
        return await base.SendAsync(request, cancellationToken);
    }
}
