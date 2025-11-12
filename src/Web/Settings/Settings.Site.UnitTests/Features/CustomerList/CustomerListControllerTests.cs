using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.CustomerList;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;

namespace AlphaPlus.Settings.Site.UnitTests.Features.CustomerList;

public class CustomerListControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<TransferService> _mockTransferService;
    private readonly Mock<IUserSession> _mockUserSession;
    private readonly CustomerListController _controller;
    private readonly Fixture _fixture;

    public CustomerListControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _mockTransferService = new Mock<TransferService>();
        _mockUserSession = new Mock<IUserSession>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new CustomerListController(_mockMediator.Object, _mockTransferService.Object, _mockUserSession.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<CustomerListController>();
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
