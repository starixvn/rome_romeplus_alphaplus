using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.ViewLogs.Controllers;

namespace AlphaPlus.Settings.Site.UnitTests.Features.ViewLogs.Controllers;

public class CashServiceControllerTests : IClassFixture<TestFixture>
{
    private readonly Fixture _fixture;

    public CashServiceControllerTests(TestFixture testFixture)
    {
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
    }

    [Fact]
    public void Controller_Type_Should_Exist()
    {
        // Assert - Test that the type exists and can be referenced
        var controllerType = typeof(CashServiceController);
        controllerType.Should().NotBeNull();
        controllerType.Name.Should().Be("CashServiceController");
    }

    [Fact]
    public void Controller_Should_Inherit_From_ViewLogBaseController()
    {
        // Assert
        var controllerType = typeof(CashServiceController);
        controllerType.BaseType.Should().Be(typeof(ViewLogBaseController));
    }

    [Fact]
    public void Controller_Should_Have_Correct_Namespace()
    {
        // Assert
        var controllerType = typeof(CashServiceController);
        controllerType.Namespace.Should().Be("AlphaPlus.Settings.Site.Features.ViewLogs.Controllers");
    }
}
