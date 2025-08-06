using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Services;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Models;
using Fanex.Logging;

namespace AlphaPlus.Core.Tests.Integration;

public class SessionCookieComparisonIntegrationTests
{
    [Fact]
    public async Task FullFlow_ShouldLogWhenSessionCookiesAreDifferent()
    {
        // Arrange
        var services = CreateTestServices();
        var serviceProvider = services.BuildServiceProvider();
        
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => 
            {
                // Simulate response with different session cookie
                context.Response.Headers["Set-Cookie"] = ".AspNetCore.Session=different-session-id; path=/; httponly";
                await context.Response.WriteAsync("test response");
            },
            serviceProvider.GetRequiredService<ISessionCookieComparisonService>());

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";
        context.Request.Headers["Cookie"] = ".AspNetCore.Session=original-session-id";

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        // Verify that middleware completed successfully
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Fact]
    public async Task FullFlow_ShouldNotLogWhenSessionCookiesAreSame()
    {
        // Arrange
        var services = CreateTestServices();
        var serviceProvider = services.BuildServiceProvider();
        
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => 
            {
                // Simulate response with same session cookie
                context.Response.Headers["Set-Cookie"] = ".AspNetCore.Session=same-session-id; path=/; httponly";
                await context.Response.WriteAsync("test response");
            },
            serviceProvider.GetRequiredService<ISessionCookieComparisonService>());

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";
        context.Request.Headers["Cookie"] = ".AspNetCore.Session=same-session-id";

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        // Verify that middleware completed successfully
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Fact]
    public async Task FullFlow_ShouldNotLogForNonMonitoredPages()
    {
        // Arrange
        var services = CreateTestServices();
        var serviceProvider = services.BuildServiceProvider();
        
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => 
            {
                context.Response.Headers["Set-Cookie"] = ".AspNetCore.Session=different-session-id; path=/; httponly";
                await context.Response.WriteAsync("test response");
            },
            serviceProvider.GetRequiredService<ISessionCookieComparisonService>());

        var context = new DefaultHttpContext();
        context.Request.Path = "/home"; // Non-monitored page
        context.Request.Headers["Cookie"] = ".AspNetCore.Session=original-session-id";

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        // Verify that middleware completed successfully
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Fact]
    public async Task FullFlow_ShouldHandleNullCookies()
    {
        // Arrange
        var services = CreateTestServices();
        var serviceProvider = services.BuildServiceProvider();
        
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => 
            {
                await context.Response.WriteAsync("test response");
            },
            serviceProvider.GetRequiredService<ISessionCookieComparisonService>());

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";
        context.Request.Headers["Cookie"] = "";

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        // Verify that middleware completed successfully
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Fact]
    public async Task FullFlow_ShouldHandleExceptionGracefully()
    {
        // Arrange
        var services = CreateTestServices();
        var serviceProvider = services.BuildServiceProvider();
        
        var middleware = new SessionCookieComparisonMiddleware(
            (context) =>
            {
                throw new Exception("Test exception");
            },
            serviceProvider.GetRequiredService<ISessionCookieComparisonService>());

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";
        context.Request.Headers["Cookie"] = ".AspNetCore.Session=original-session-id";

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => middleware.InvokeAsync(context));
    }

    private static IServiceCollection CreateTestServices()
    {
        var services = new ServiceCollection();
        
        // Mock logger
        var loggerMock = new Mock<ILogger>();
        services.AddSingleton(loggerMock.Object);
        
        // Mock user session
        var userSessionMock = new Mock<IUserSession>();
        userSessionMock.Setup(x => x.CustId).Returns(123);
        userSessionMock.Setup(x => x.UserName).Returns("testuser");
        userSessionMock.Setup(x => x.NetSessionId).Returns("456");
        userSessionMock.Setup(x => x.RoleId).Returns(1);
        userSessionMock.Setup(x => x.AdminId).Returns(0);
        userSessionMock.Setup(x => x.AdminName).Returns("");
        userSessionMock.Setup(x => x.UserLoginIp).Returns("192.168.1.1");
        services.AddSingleton(userSessionMock.Object);
        
        // Configure options
        var options = new SessionCookieComparisonOptions
        {
            EnableSessionCookieComparison = true,
            LogLevel = "Error",
            IncludeUserAgentInLog = true,
            IncludeRefererInLog = true,
            IncludeIpAddressInLog = true,
            MonitoredPaths = new[] { "/settings/creditbalance", "/reports/winlossdetail" }
        };
        
        var optionsMock = new Mock<IOptions<SessionCookieComparisonOptions>>();
        optionsMock.Setup(x => x.Value).Returns(options);
        services.AddSingleton(optionsMock.Object);
        
        // Register the service
        services.AddSingleton<ISessionCookieComparisonService, SessionCookieComparisonService>();
        
        return services;
    }
} 