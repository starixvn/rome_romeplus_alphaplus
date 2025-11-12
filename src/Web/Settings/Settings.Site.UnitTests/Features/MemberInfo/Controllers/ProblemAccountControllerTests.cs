using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
using AlphaPlus.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.Site.UnitTests.Features.MemberInfo.Controllers;

public class ProblemAccountControllerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly ProblemAccountController _controller;
    private readonly Fixture _fixture;

    public ProblemAccountControllerTests(TestFixture testFixture)
    {
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        TestFixture.ConfigureGlobalAutoFixture(_fixture);
        _controller = new ProblemAccountController(_mockMediator.Object);
        
        // Setup controller context
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext(),
            ActionDescriptor = new ControllerActionDescriptor(),
            RouteData = new RouteData()
        };
    }

    private IndexRequest CreateIndexRequest()
    {
        var mockUser = new Mock<IUser>();
        mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
        mockUser.SetupGet(u => u.Id).Returns(456);
        mockUser.SetupGet(u => u.Name).Returns("testuser");
        mockUser.SetupGet(u => u.Site).Returns("testsite");
        mockUser.SetupGet(u => u.LevelId).Returns((byte)2); // Agent level
        mockUser.SetupGet(u => u.SubAccId).Returns(789);
        mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
        mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
        
        return new IndexRequest(mockUser.Object);
    }

    private UpdateProblemAccountRequest CreateUpdateProblemAccountRequest()
    {
        var mockUser = new Mock<IUser>();
        mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
        mockUser.SetupGet(u => u.Id).Returns(456);
        mockUser.SetupGet(u => u.Name).Returns("testuser");
        mockUser.SetupGet(u => u.Site).Returns("testsite");
        mockUser.SetupGet(u => u.LevelId).Returns((byte)2);
        mockUser.SetupGet(u => u.SubAccId).Returns(789);
        mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
        mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
        
        return new UpdateProblemAccountRequest(mockUser.Object);
    }

    private UpdateProblemAccountCommand CreateUpdateProblemAccountCommand()
    {
        var mockUser = new Mock<IUser>();
        mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
        mockUser.SetupGet(u => u.Id).Returns(456);
        mockUser.SetupGet(u => u.Name).Returns("testuser");
        mockUser.SetupGet(u => u.Site).Returns("testsite");
        mockUser.SetupGet(u => u.LevelId).Returns((byte)2);
        mockUser.SetupGet(u => u.SubAccId).Returns(789);
        mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
        mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
        
        return new UpdateProblemAccountCommand(mockUser.Object);
    }

    private GetProblemAccountRequest CreateGetProblemAccountRequest()
    {
        var mockUser = new Mock<IUser>();
        mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
        mockUser.SetupGet(u => u.Id).Returns(456);
        mockUser.SetupGet(u => u.Name).Returns("testuser");
        mockUser.SetupGet(u => u.Site).Returns("testsite");
        mockUser.SetupGet(u => u.LevelId).Returns((byte)2);
        mockUser.SetupGet(u => u.SubAccId).Returns(789);
        mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
        mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
        
        return new GetProblemAccountRequest(mockUser.Object);
    }

    private ProblemAccountExcelQuery CreateProblemAccountExcelQuery()
    {
        var mockUser = new Mock<IUser>();
        mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
        mockUser.SetupGet(u => u.Id).Returns(456);
        mockUser.SetupGet(u => u.Name).Returns("testuser");
        mockUser.SetupGet(u => u.Site).Returns("testsite");
        mockUser.SetupGet(u => u.LevelId).Returns((byte)2);
        mockUser.SetupGet(u => u.SubAccId).Returns(789);
        mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
        mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
        
        return new ProblemAccountExcelQuery(mockUser.Object);
    }

    [Fact]
    public async Task Index_Should_Return_ViewResult_With_ViewModel()
    {
        // Arrange
        var request = CreateIndexRequest();
        var expectedViewModel = _fixture.Create<ProblemAccountReportViewModel>();
        
        _mockMediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedViewModel);

        // Act
        var result = await _controller.Index(request);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult.ViewName.Should().Be("Index");
        _mockMediator.Verify(m => m.Send(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_Should_Return_ViewResult_With_ViewModel()
    {
        // Arrange
        var request = CreateUpdateProblemAccountRequest();
        var expectedViewModel = _fixture.Create<UpdateReviewStatusViewModel>();
        
        _mockMediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedViewModel);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult.ViewName.Should().Be("Update");
        _mockMediator.Verify(m => m.Send(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProblemAccounts_Should_Return_JsonResult()
    {
        // Arrange
        var request = CreateGetProblemAccountRequest();
        var expectedData = _fixture.CreateMany<ProblemAccountModel>(3).ToList();
        
        _mockMediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetProblemAccounts(request);

        // Assert
        result.Should().BeOfType<JsonResult>();
        _mockMediator.Verify(m => m.Send(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCommand_Should_Return_JsonResult()
    {
        // Arrange
        var command = CreateUpdateProblemAccountCommand();
        var expectedResult = _fixture.Create<Result>();
        
        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.UpdateCommand(command);

        // Assert
        result.Should().BeOfType<JsonResult>();
        _mockMediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Excel_Should_Return_ExcelView()
    {
        // Arrange
        var query = CreateProblemAccountExcelQuery();
        var expectedData = _fixture.Create<ExcelFileViewModel>();
        
        _mockMediator.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedData);

        // Act
        var result = await _controller.Excel(query);

        // Assert
        result.Should().NotBeNull();
        _mockMediator.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Index_Should_Handle_Invalid_Request_Gracefully(string invalidValue)
    {
        // Arrange
        var request = CreateIndexRequest();
        request.UserName = invalidValue;
        var expectedViewModel = _fixture.Create<ProblemAccountReportViewModel>();
        
        _mockMediator.Setup(m => m.Send(It.IsAny<IndexRequest>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedViewModel);

        // Act
        var result = await _controller.Index(request);

        // Assert
        result.Should().BeOfType<ViewResult>();
        _mockMediator.Verify(m => m.Send(It.IsAny<IndexRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCommand_Should_Handle_Mediator_Exception()
    {
        // Arrange
        var command = CreateUpdateProblemAccountCommand();
        
        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                   .ThrowsAsync(new InvalidOperationException("Test exception"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.UpdateCommand(command));
        _mockMediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Controller_Should_Have_Correct_Constructor_Dependencies()
    {
        // Arrange & Act
        var controller = new ProblemAccountController(_mockMediator.Object);

        // Assert
        controller.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProblemAccounts_Should_Handle_Empty_Result()
    {
        // Arrange
        var request = CreateGetProblemAccountRequest();
        var emptyData = new List<ProblemAccountModel>();
        
        _mockMediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(emptyData);

        // Act
        var result = await _controller.GetProblemAccounts(request);

        // Assert
        result.Should().BeOfType<JsonResult>();
        _mockMediator.Verify(m => m.Send(request, It.IsAny<CancellationToken>()), Times.Once);
    }
}
