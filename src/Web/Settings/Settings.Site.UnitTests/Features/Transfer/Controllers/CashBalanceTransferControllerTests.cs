using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.Transfer.Controllers;

namespace AlphaPlus.Settings.Site.UnitTests.Features.Transfer.Controllers;

public class CashBalanceTransferControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly CashBalanceTransferController _controller;
    private readonly Fixture _fixture;

    public CashBalanceTransferControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new CashBalanceTransferController(_mockMediator.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<CashBalanceTransferController>();
    }

    [Fact]
    public void Controller_Should_Have_Mediator_Dependency()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_TransferBaseController()
    {
        // Assert
        _controller.Should().BeAssignableTo<AlphaPlus.Settings.Site.Features.Transfer.Controllers.TransferBaseController>();
    }
}
