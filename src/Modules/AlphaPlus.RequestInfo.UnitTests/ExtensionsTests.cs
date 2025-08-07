using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.RequestInfo.UnitTests;

public class ExtensionsTests
{
    [Fact]
    public void AddRequestInfo_ShouldRegisterAllRequiredServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMemoryCache(); // Add memory cache for IPService

        // Act
        services.AddRequestInfo();

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify HttpContextAccessor is registered
        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        Assert.NotNull(httpContextAccessor);

        // Verify IDynamicRepository is registered
        var dynamicRepository = serviceProvider.GetService<IDynamicRepository>();
        Assert.NotNull(dynamicRepository);

        // Verify IPService is registered
        var ipService = serviceProvider.GetService<IPService>();
        Assert.NotNull(ipService);

        // Verify IPHelper is registered
        var ipHelper = serviceProvider.GetService<IPHelper>();
        Assert.NotNull(ipHelper);

        // Verify IDeviceHelper is registered
        var deviceHelper = serviceProvider.GetService<IDeviceHelper>();
        Assert.NotNull(deviceHelper);
    }

    [Fact]
    public void AddRequestInfo_ShouldReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddRequestInfo();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddRequestInfo_ShouldRegisterServicesAsSingletons()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMemoryCache(); // Add memory cache for IPService

        // Act
        services.AddRequestInfo();

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Get first instance
        var deviceHelper1 = serviceProvider.GetService<IDeviceHelper>();
        var deviceHelper2 = serviceProvider.GetService<IDeviceHelper>();

        // Verify they are the same instance (singleton)
        Assert.Same(deviceHelper1, deviceHelper2);
    }
}