//using AlphaPlus.Reports.Site.Features.WinLossByProduct;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Reports.Site.UnitTests.Features.WinLossByProduct;

//public class WinLossByProductControllerTests
//{
//    private readonly Mock<IMediator> _mediatorMock;
//    private readonly WinLossByProductController _controller;

//    public WinLossByProductControllerTests()
//    {
//        _mediatorMock = new Mock<IMediator>();
//        _controller = new WinLossByProductController(_mediatorMock.Object);
//    }

//    [Fact]
//    public async Task Index_ShouldReturnViewResult()
//    {
//        // Arrange
//        var query = new WinLossByProductQuery();

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
//        var query = new WinLossByProductExcelQuery();
//        _mediatorMock.Setup(m => m.Send(It.IsAny<WinLossByProductExcelQuery>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new byte[] { 1, 2, 3 });

//        // Act
//        var result = await _controller.Excel(query);

//        // Assert
//        var fileResult = Assert.IsType<FileContentResult>(result);
//        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
//        Assert.NotEmpty(fileResult.FileContents);
//    }
//}