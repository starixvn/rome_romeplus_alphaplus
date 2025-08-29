using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using DomainUser = AlphaPlus.Settings.MemberInfo.Domain.Models.IUser;

namespace AlphaPlus.Settings.Site.UnitTests.Features.MemberInfo.Controllers;

public class WinLossLimitControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IUserSession> _mockUserSession;
    private readonly Mock<IAntiforgery> _mockAntiforgery;
    private readonly Mock<DomainUser> _mockUser;
    private readonly WinLossLimitController _controller;
    private readonly Fixture _fixture;

    public WinLossLimitControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _mockUserSession = new Mock<IUserSession>();
        _mockAntiforgery = new Mock<IAntiforgery>();
        _mockUser = new Mock<DomainUser>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new WinLossLimitController(
            _mockMediator.Object,
            _mockUserSession.Object,
            _mockAntiforgery.Object,
            _mockUser.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<WinLossLimitController>();
    }

    [Fact]
    public void Controller_Should_Have_All_Dependencies()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
        _mockUserSession.Should().NotBeNull();
        _mockAntiforgery.Should().NotBeNull();
        _mockUser.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_SiteBaseController()
    {
        // Assert
        _controller.Should().BeAssignableTo<SiteBaseController>();
    }
}
