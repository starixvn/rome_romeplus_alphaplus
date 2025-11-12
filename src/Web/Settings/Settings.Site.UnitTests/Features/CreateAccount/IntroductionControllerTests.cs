using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.CreateAccount;
using AlphaPlus.Core;

namespace AlphaPlus.Settings.Site.UnitTests.Features.CreateAccount;

public class IntroductionControllerTests : IClassFixture<TestFixture>
{
    private readonly Fixture _fixture;

    public IntroductionControllerTests(TestFixture testFixture)
    {
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
    }

    [Fact]
    public void Controller_Type_Should_Exist()
    {
        // Assert - Test that the type exists and can be referenced
        var controllerType = typeof(IntroductionController);
        controllerType.Should().NotBeNull();
        controllerType.Name.Should().Be("IntroductionController");
    }

    [Fact]
    public void Controller_Should_Inherit_From_BaseController()
    {
        // Assert
        var controllerType = typeof(IntroductionController);
        controllerType.BaseType.Should().Be(typeof(BaseController));
    }

    [Fact]
    public void Controller_Should_Have_Correct_Namespace()
    {
        // Assert
        var controllerType = typeof(IntroductionController);
        controllerType.Namespace.Should().Be("AlphaPlus.Settings.Site.Features.CreateAccount");
    }
}
