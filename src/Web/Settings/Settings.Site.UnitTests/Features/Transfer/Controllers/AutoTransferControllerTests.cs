using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.Transfer.Controllers;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using AlphaPlus.Settings.Transfer.API.Services;

namespace AlphaPlus.Settings.Site.UnitTests.Features.Transfer.Controllers;

public class AutoTransferControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IUserSession> _mockUserSession;
    private readonly Mock<ITransferService> _mockTransferService;
    private readonly AutoTransferController _controller;
    private readonly Fixture _fixture;

    public AutoTransferControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _mockUserSession = new Mock<IUserSession>();
        _mockTransferService = new Mock<ITransferService>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new AutoTransferController(
            _mockMediator.Object,
            _mockUserSession.Object,
            _mockTransferService.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<AutoTransferController>();
    }

    [Fact]
    public void Controller_Should_Have_All_Dependencies()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
        _mockUserSession.Should().NotBeNull();
        _mockTransferService.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_Inherit_From_TransferBaseController()
    {
        // Assert
        _controller.Should().BeAssignableTo<TransferBaseController>();
    }
}
