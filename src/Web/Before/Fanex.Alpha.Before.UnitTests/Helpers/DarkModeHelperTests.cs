using Fanex.Alpha.Before.Helpers;
using Fanex.Alpha.Before.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Http;

namespace Fanex.Alpha.Before.UnitTests.Helpers;

public class DarkModeHelperTests
{
    [Fact]
    public void GetDarkModeValue_WhenCookieIs1_ReturnsDarkSuffix()
    {
        var context = new DefaultHttpContext();
        context.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "ISDARKMODE", "1" } });

        var result = DarkModeHelper.GetDarkModeValue(context.Request);

        Assert.Equal("-dark", result);
    }

    [Fact]
    public void GetDarkModeValue_WhenCookieIsMissingOrNot1_ReturnsEmpty()
    {
        var context = new DefaultHttpContext();
        context.Request.Cookies = new TestRequestCookies();

        var result = DarkModeHelper.GetDarkModeValue(context.Request);

        Assert.Equal(string.Empty, result);

        context.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "ISDARKMODE", "0" } });

        result = DarkModeHelper.GetDarkModeValue(context.Request);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void SetDarkModeSession_SetsAppSessionDarkModeName()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestDoubles.TestSession();
        httpContext.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "ISDARKMODE", "1" } });

        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        HttpContextHelper.Configure(accessor);

        DarkModeHelper.SetDarkModeSession(httpContext.Request);

        Assert.Equal("-dark", AppSession.DarkModeName);
    }
}


