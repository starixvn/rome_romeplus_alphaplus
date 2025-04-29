using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Controllers;

public class HomeControllerTests : TestBase
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Arrange
        // Add your controller setup here

        // Act
        // Add your test action here

        // Assert
        // Add your assertions here
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        // Add controller-specific service configurations here
    }
}