using System.Text;
using Fanex.Alpha.Before.Helpers;
using Fanex.Alpha.Before.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Fanex.Alpha.Before.UnitTests.Helpers;

public class ExceptionHelperTests
{
    [Fact]
    public async Task BuildError_IncludesPathHeadersCookiesAndBody()
    {
        var context = new DefaultHttpContext();
        var body = "{\"x\":1}";
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("example.com");
        context.Request.PathBase = new PathString("/base");
        context.Request.Path = new PathString("/path");
        context.Request.QueryString = new QueryString("?q=1");
        context.Request.Headers["h1"] = "v1";
        context.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "c1", "v1" } });
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));

        var exFeature = new ExceptionHandlerFeature
        {
            Error = new InvalidOperationException("boom"),
            Path = "/requested"
        };

        var result = await ExceptionHelper.BuildError(context.Request, exFeature);

        Assert.Contains("boom", result);
        Assert.Contains("PATH: https://example.com/base/path?q=1", result);
        Assert.Contains("REQUESTED URL: /requested", result);
        Assert.Contains("BODY: " + body, result);
        Assert.Contains("h1:v1", result);
        Assert.Contains("c1:v1", result);
        Assert.Contains("EXCEPTION:", result);
    }
}


