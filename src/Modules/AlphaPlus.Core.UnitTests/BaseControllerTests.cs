using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Core.UnitTests;

public class BaseControllerTests
{
    [Fact]
    public void BaseController_Should_Inherit_From_Controller()
    {
        // Act
        var controller = new TestBaseController();

        // Assert
        Assert.IsAssignableFrom<Controller>(controller);
    }

    [Fact]
    public void BaseController_Should_Have_Authorize_Attribute()
    {
        // Arrange
        var controllerType = typeof(BaseController);

        // Act
        var authorizeAttribute = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .FirstOrDefault() as AuthorizeAttribute;

        // Assert
        Assert.NotNull(authorizeAttribute);
    }

    [Fact]
    public void BaseController_Should_Be_Public()
    {
        // Arrange
        var controllerType = typeof(BaseController);

        // Act
        var isPublic = controllerType.IsPublic;

        // Assert
        Assert.True(isPublic);
    }

    [Fact]
    public void BaseController_Should_Not_Be_Abstract()
    {
        // Arrange
        var controllerType = typeof(BaseController);

        // Act
        var isAbstract = controllerType.IsAbstract;

        // Assert
        Assert.False(isAbstract);
    }

    [Fact]
    public void BaseController_Should_Not_Be_Sealed()
    {
        // Arrange
        var controllerType = typeof(BaseController);

        // Act
        var isSealed = controllerType.IsSealed;

        // Assert
        Assert.False(isSealed);
    }

    [Fact]
    public void BaseController_Should_Be_Inheritable()
    {
        // Act
        var derivedController = new TestBaseController();

        // Assert
        Assert.IsType<TestBaseController>(derivedController);
        Assert.IsAssignableFrom<BaseController>(derivedController);
    }

    private class TestBaseController : BaseController
    {
        // Test implementation
    }
}
