//using AlphaPlus.Reports.Site.Features.WinLossDetail;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Reports.Site.UnitTests.Features.WinLossDetail;

//public class WinLossDetailControllerTests
//{
//    private readonly Mock<IMediator> _mediatorMock;
//    private readonly WinLossDetailController _controller;

//    public WinLossDetailControllerTests()
//    {
//        _mediatorMock = new Mock<IMediator>();
//        _controller = new WinLossDetailController(_mediatorMock.Object);
//    }

//    [Fact]
//    public async Task Index_ShouldReturnViewResult()
//    {
//        // Arrange
//        var customerId = "testCustomerId";
//        var query = new WinLossDetailQuery
//        {
//            CustomerId = customerId
//        };

//        // Act
//        var result = await _controller.Index(query);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.Equal(query, viewResult.Model);
//    }

//    [Fact]
//    public async Task Excel_ShouldReturnFileResult()
//    {
//        // Arrange
//        var customerId = "testCustomerId";
//        var query = new WinLossDetailQuery
//        {
//            CustomerId = customerId
//        };

//        _mediatorMock.Setup(m => m.Send(It.IsAny<WinLossDetailQuery>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new byte[] { 1, 2, 3 });

//        // Act
//        var result = await _controller.Excel(query);

//        // Assert
//        var fileResult = Assert.IsType<FileContentResult>(result);
//        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
//        Assert.NotEmpty(fileResult.FileContents);
//    }
//}