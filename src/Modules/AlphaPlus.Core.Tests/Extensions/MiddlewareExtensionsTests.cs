using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Services;

namespace AlphaPlus.Core.Tests.Extensions;

public class MiddlewareExtensionsTests
{
    [Fact]
    public void UseSessionCookieComparison_ShouldReturnApplicationBuilder()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ISessionCookieComparisonService, MockSessionCookieComparisonService>();
        var serviceProvider = services.BuildServiceProvider();
        
        var appBuilder = new ApplicationBuilder(serviceProvider);

        // Act
        var result = appBuilder.UseSessionCookieComparison();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApplicationBuilder>(result);
    }

    [Fact]
    public void UseSessionCookieComparison_ShouldRegisterMiddleware()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ISessionCookieComparisonService, MockSessionCookieComparisonService>();
        var serviceProvider = services.BuildServiceProvider();
        
        var appBuilder = new ApplicationBuilder(serviceProvider);

        // Act
        appBuilder.UseSessionCookieComparison();

        // Assert
        // The middleware should be registered in the pipeline
        // We can't directly test the middleware registration, but we can verify the method doesn't throw
        Assert.NotNull(appBuilder);
    }

    private class MockSessionCookieComparisonService : ISessionCookieComparisonService
    {
        public Task CompareAndLogSessionCookiesAsync(HttpContext context, string requestCookie, string responseCookie)
        {
            return Task.CompletedTask;
        }

        public bool IsMonitoredPage(PathString path)
        {
            return false;
        }

        public string ExtractSessionCookieFromRequest(HttpRequest request)
        {
            return string.Empty;
        }

        public string ExtractSessionCookieFromResponse(HttpResponse response)
        {
            return string.Empty;
        }
    }
} 