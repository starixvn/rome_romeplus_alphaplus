using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.UserNameSuggestion;
using AlphaPlus.Core;

namespace AlphaPlus.Settings.Site.UnitTests.Features.UserNameSuggestion;

public class UserNameSuggestionControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly UserNameSuggestionController _controller;
    private readonly Fixture _fixture;

    public UserNameSuggestionControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new UserNameSuggestionController(_mockMediator.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<UserNameSuggestionController>();
    }

    [Fact]
    public void Controller_Should_Have_Mediator_Dependency()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_BaseController()
    {
        // Assert
        _controller.Should().BeAssignableTo<BaseController>();
    }
}
