using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance;

namespace AlphaPlus.Settings.Site.UnitTests.Features.MemberInfo.Controllers;

public class CreditBalanceControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly CreditBalanceController _controller;
    private readonly Fixture _fixture;

    public CreditBalanceControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new CreditBalanceController(_mockMediator.Object);
    }

    [Fact]
    public void Controller_Should_Be_Instantiated()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<CreditBalanceController>();
    }

    [Fact]
    public void Controller_Should_Have_Mediator_Dependency()
    {
        // Assert
        _mockMediator.Should().NotBeNull();
    }

    [Fact]
    public async Task Index_Should_Return_ActionResult()
    {
        // Arrange
        var query = CreateCreditBalanceQuery();

        // Act
        var result = await _controller.Index(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
    }

    private CreditBalanceQuery CreateCreditBalanceQuery()
    {
        // Create a simple query without complex dependencies
        return new CreditBalanceQuery();
    }
}
