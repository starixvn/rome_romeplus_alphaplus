using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.CopyAccount;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.UnitTests.Features.CopyAccount;

public class CopyAccountControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly CopyAccountController _controller;
    private readonly Fixture _fixture;

    public CopyAccountControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new CopyAccountController(_mockMediator.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<CopyAccountController>();
    }

    [Fact]
    public void Controller_Should_Have_Mediator_Dependency()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_SiteBaseController()
    {
        // Assert
        _controller.Should().BeAssignableTo<SiteBaseController>();
    }
}
