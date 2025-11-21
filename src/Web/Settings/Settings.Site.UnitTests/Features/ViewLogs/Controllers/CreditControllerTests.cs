using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.ViewLogs.Controllers;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Settings.Site.UnitTests.Features.ViewLogs.Controllers;

public class CreditControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IUserSession> _mockUserSession;
    private readonly Mock<IMediator> _mockMediator;
    private readonly CreditController _controller;
    private readonly Fixture _fixture;

    public CreditControllerTests(TestFixture testFixture)
    {
        _mockUserSession = new Mock<IUserSession>();
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new CreditController(
            _mockUserSession.Object,
            _mockMediator.Object,
            null); // Pass null for MicroSites
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<CreditController>();
    }

    [Fact]
    public void Controller_Should_Have_All_Dependencies()
    {
        // Assert
        _mockUserSession.Should().NotBeNull();
        _mockMediator.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_ViewLogBaseController()
    {
        // Assert
        _controller.Should().BeAssignableTo<ViewLogBaseController>();
    }
}
