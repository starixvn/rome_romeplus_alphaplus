using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Services;
using AlphaPlus.Core.Configurations;
using Moq;
using Fanex.Logging;

namespace AlphaPlus.Core.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAlphaPlusCore_ShouldRegisterSessionCookieComparisonService()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateTestConfiguration();
        
        // Mock ILogger
        var loggerMock = new Mock<ILogger>();
        services.AddSingleton(loggerMock.Object);

        // Act
        services.AddAlphaPlusCore(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var sessionCookieComparisonService = serviceProvider.GetService<ISessionCookieComparisonService>();
        Assert.NotNull(sessionCookieComparisonService);
        Assert.IsType<SessionCookieComparisonService>(sessionCookieComparisonService);
    }

    [Fact]
    public void AddAlphaPlusCore_ShouldConfigureSessionCookieComparisonOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateTestConfiguration();
        
        // Mock ILogger
        var loggerMock = new Mock<ILogger>();
        services.AddSingleton(loggerMock.Object);

        // Act
        services.AddAlphaPlusCore(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<SessionCookieComparisonOptions>>();
        Assert.NotNull(options);
        
        var optionsValue = options.Value;
        Assert.True(optionsValue.EnableSessionCookieComparison);
        Assert.Equal("Error", optionsValue.LogLevel);
        Assert.True(optionsValue.IncludeUserAgentInLog);
        Assert.True(optionsValue.IncludeRefererInLog);
        Assert.True(optionsValue.IncludeIpAddressInLog);
        Assert.NotNull(optionsValue.MonitoredPaths);
    }

    [Fact]
    public void AddAlphaPlusCore_ShouldRegisterAllRequiredServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateTestConfiguration();
        
        // Mock ILogger
        var loggerMock = new Mock<ILogger>();
        services.AddSingleton(loggerMock.Object);

        // Act
        services.AddAlphaPlusCore(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        
        // Check that all required services are registered
        Assert.NotNull(serviceProvider.GetService<ISessionCookieComparisonService>());
        Assert.NotNull(serviceProvider.GetService<IUserSession>());
        Assert.NotNull(serviceProvider.GetService<IRequestAnalyst>());
        Assert.NotNull(serviceProvider.GetService<IRequestContext>());
        Assert.NotNull(serviceProvider.GetService<IAuthorizationService>());
    }

    [Fact]
    public void AddAlphaPlusCore_ShouldRegisterServicesAsSingletons()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateTestConfiguration();
        
        // Mock ILogger
        var loggerMock = new Mock<ILogger>();
        services.AddSingleton(loggerMock.Object);

        // Act
        services.AddAlphaPlusCore(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        
        // Get the same service twice and verify they are the same instance (singleton)
        var service1 = serviceProvider.GetService<ISessionCookieComparisonService>();
        var service2 = serviceProvider.GetService<ISessionCookieComparisonService>();
        
        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Same(service1, service2);
    }

    private static IConfiguration CreateTestConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "SiteLabel", "TestSite" },
            { "RedisConfiguration", "localhost:6379" },
            { "Authorization:SomeKey", "SomeValue" }
        });
        
        return configurationBuilder.Build();
    }
} 