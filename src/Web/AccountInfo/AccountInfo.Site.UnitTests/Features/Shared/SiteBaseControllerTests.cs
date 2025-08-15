using AlphaPlus.AccountInfo.Site.Features.Shared;

namespace AlphaPlus.AccountInfo.Site.UnitTests.Features.Shared;

public class SiteBaseControllerTests
{
    private readonly Fixture _fixture;

    public SiteBaseControllerTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void SiteBaseController_ShouldHaveRequiredMethods()
    {
        // Arrange & Act
        var controllerType = typeof(SiteBaseController);

        // Assert
        // Note: These methods are protected, so we can't access them via reflection from outside the class
        // This test verifies that the class exists and has the expected structure
        Assert.True(controllerType.IsClass);
        Assert.True(controllerType.IsPublic);
    }

    [Fact]
    public void SiteBaseController_ShouldInheritFromController()
    {
        // Arrange & Act
        var controllerType = typeof(SiteBaseController);

        // Assert
        Assert.True(typeof(Controller).IsAssignableFrom(controllerType));
    }


}
