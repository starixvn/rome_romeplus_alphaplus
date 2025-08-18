namespace AlphaPlus.SecurityCode.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddSecurityCodeServices_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();

        configuration.Setup(x => x.GetSection(SecurityCodeOptions.SecurityCode))
            .Returns(configurationSection.Object);

        // Act
        var result = services.AddSecurityCodeServices(configuration.Object);

        // Assert
        Assert.Same(services, result);

        // Verify that all services are registered
        var serviceProvider = services.BuildServiceProvider();

        // These should not throw exceptions when resolved
        Assert.NotNull(serviceProvider.GetService<ISecurityCodeHttpContext>());
        Assert.NotNull(serviceProvider.GetService<ISecurityCodeChecker>());
        Assert.NotNull(serviceProvider.GetService<ISecurityCodePathBuilder>());
    }

    [Fact]
    public void AddSecurityCodeServices_ShouldConfigureSecurityCodeOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();

        configuration.Setup(x => x.GetSection(SecurityCodeOptions.SecurityCode))
            .Returns(configurationSection.Object);

        // Act
        services.AddSecurityCodeServices(configuration.Object);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<SecurityCodeOptions>>();

        Assert.NotNull(options);
        Assert.NotNull(options.Value);
    }

    [Fact]
    public void AddSecurityCodeServices_ShouldRegisterServicesAsSingletons()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();

        configuration.Setup(x => x.GetSection(SecurityCodeOptions.SecurityCode))
            .Returns(configurationSection.Object);

        // Act
        services.AddSecurityCodeServices(configuration.Object);

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Get the same service twice to verify singleton behavior
        var httpContext1 = serviceProvider.GetService<ISecurityCodeHttpContext>();
        var httpContext2 = serviceProvider.GetService<ISecurityCodeHttpContext>();

        var checker1 = serviceProvider.GetService<ISecurityCodeChecker>();
        var checker2 = serviceProvider.GetService<ISecurityCodeChecker>();

        var pathBuilder1 = serviceProvider.GetService<ISecurityCodePathBuilder>();
        var pathBuilder2 = serviceProvider.GetService<ISecurityCodePathBuilder>();

        Assert.Same(httpContext1, httpContext2);
        Assert.Same(checker1, checker2);
        Assert.Same(pathBuilder1, pathBuilder2);
    }

    [Fact]
    public void AddSecurityCodeServices_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        IServiceCollection services = null!;
        var configuration = new Mock<IConfiguration>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddSecurityCodeServices(configuration.Object));
    }
}
