//using AlphaPlus.Reports.Site.Features.WinLossAnalysis;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Reports.Site.UnitTests.Features.WinLossAnalysis;

//public class WinLossAnalysisControllerTests
//{
//    private readonly Mock<IMediator> _mediatorMock;
//    private readonly WinLossAnalysisController _controller;

//    public WinLossAnalysisControllerTests()
//    {
//        _mediatorMock = new Mock<IMediator>();
//        _controller = new WinLossAnalysisController(_mediatorMock.Object);
//    }

//    [Fact]
//    public async Task Index_ShouldReturnViewResult()
//    {
//        // Arrange
//        var query = new WinLossAnalysisQuery();

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
//        var query = new WinLossAnalysisExcelQuery();
//        _mediatorMock.Setup(m => m.Send(It.IsAny<WinLossAnalysisExcelQuery>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new byte[] { 1, 2, 3 });

//        // Act
//        var result = await _controller.Excel(query);

//        // Assert
//        var fileResult = Assert.IsType<FileContentResult>(result);
//        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
//        Assert.NotEmpty(fileResult.FileContents);
//    }
//}