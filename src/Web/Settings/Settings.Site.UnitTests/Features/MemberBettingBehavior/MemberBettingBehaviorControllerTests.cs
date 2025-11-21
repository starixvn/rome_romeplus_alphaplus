using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.UnitTests.Features.MemberBettingBehavior;

public class MemberBettingBehaviorControllerTests : IClassFixture<TestFixture>
{
    private readonly Fixture _fixture;

    public MemberBettingBehaviorControllerTests(TestFixture testFixture)
    {
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
    }

    [Fact]
    public void Controller_Type_Should_Exist()
    {
        // Assert - Test that the type exists and can be referenced
        var controllerType = typeof(MemberBettingBehaviorController);
        controllerType.Should().NotBeNull();
        controllerType.Name.Should().Be("MemberBettingBehaviorController");
    }

    [Fact]
    public void Controller_Should_Inherit_From_SiteBaseController()
    {
        // Assert
        var controllerType = typeof(MemberBettingBehaviorController);
        controllerType.BaseType.Should().Be(typeof(SiteBaseController));
    }

    [Fact]
    public void Controller_Should_Have_Correct_Namespace()
    {
        // Assert
        var controllerType = typeof(MemberBettingBehaviorController);
        controllerType.Namespace.Should().Be("AlphaPlus.Settings.Site.Features.MemberBettingBehavior");
    }
}
