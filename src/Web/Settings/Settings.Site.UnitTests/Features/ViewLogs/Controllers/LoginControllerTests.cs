using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.ViewLogs.Controllers;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Settings.Site.UnitTests.Features.ViewLogs.Controllers;

public class LoginControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IUserSession> _mockUserSession;
    private readonly Mock<IMediator> _mockMediator;
    private readonly LoginController _controller;
    private readonly Fixture _fixture;

    public LoginControllerTests(TestFixture testFixture)
    {
        _mockUserSession = new Mock<IUserSession>();
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new LoginController(_mockUserSession.Object, _mockMediator.Object, null); // Pass null for MicroSites
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<LoginController>();
    }

    [Fact]
    public void Controller_Should_Have_Mediator_Dependency()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_Controller()
    {
        // Assert
        _controller.Should().BeAssignableTo<Controller>();
    }
}
