using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.ProductStatus;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.UnitTests.Features.ProductStatus;

public class ProductStatusControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly ProductStatusController _controller;
    private readonly Fixture _fixture;

    public ProductStatusControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new ProductStatusController(_mockMediator.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<ProductStatusController>();
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
