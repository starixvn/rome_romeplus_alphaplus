using System.Net;
using System.Text.Json;
using Fanex.Alpha.Before.Extensions;

namespace Fanex.Alpha.Before.UnitTests.Extensions;

public class HttpClientExtensionsTests
{
    [Fact]
    public async Task PostAsJsonAsync_SerializesAndDeserializes()
    {
        var handler = new FakeHandler(_ =>
        {
            var obj = new Payload { Name = "ok" };
            var json = JsonSerializer.Serialize(obj);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
        });

        var client = new HttpClient(handler);

        var result = await client.PostAsJsonAsync<Payload>("http://localhost/api", new { A = 1 });

        Assert.NotNull(result);
        Assert.Equal("ok", result!.Name);
    }

    private sealed class FakeHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> responder;
        public FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) => this.responder = responder;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(responder(request));
    }

    private sealed record Payload
    {
        public string Name { get; init; } = string.Empty;
    }
}


