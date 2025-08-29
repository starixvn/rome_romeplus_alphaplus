using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.PositionTaking.Controllers;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Core;

namespace AlphaPlus.Settings.Site.UnitTests.Features.PositionTaking.Controllers;

public class PositionTakingControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IProductService> _mockProductService;
    private readonly PositionTakingController _controller;
    private readonly Fixture _fixture;

    public PositionTakingControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _mockProductService = new Mock<IProductService>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new PositionTakingController(_mockMediator.Object, _mockProductService.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<PositionTakingController>();
    }

    [Fact]
    public void Controller_Should_Have_Dependencies()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
        _mockProductService.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_BaseController()
    {
        // Assert
        _controller.Should().BeAssignableTo<BaseController>();
    }
}
