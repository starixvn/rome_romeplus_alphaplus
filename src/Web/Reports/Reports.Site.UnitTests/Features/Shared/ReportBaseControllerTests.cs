//using AlphaPlus.Reports.Site.Features.Shared;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Reports.Site.UnitTests.Features.Shared;

//public class ReportBaseControllerTests
//{
//    private class TestQuery { }

//    private class TestController : ReportBaseController
//    {
//        public TestController(IMediator mediator) : base(mediator)
//        {
//        }

//        public Task<IActionResult> TestView(object model) => View(model);

//        public Task<IActionResult> TestExcelView(object query) => ExcelView(query);
//    }

//    private readonly Mock<IMediator> _mediatorMock;
//    private readonly TestController _controller;

//    public ReportBaseControllerTests()
//    {
//        _mediatorMock = new Mock<IMediator>();
//        _controller = new TestController(_mediatorMock.Object);
//    }

//    [Fact]
//    public async Task View_ShouldReturnViewResult()
//    {
//        // Arrange
//        var model = new TestQuery();

//        // Act
//        var result = await _controller.TestView(model);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.Equal(model, viewResult.Model);
//    }

//    [Fact]
//    public async Task ExcelView_ShouldReturnFileContentResult()
//    {
//        // Arrange
//        var query = new TestQuery();
//        var excelData = new byte[] { 1, 2, 3 };

//        _mediatorMock.Setup(m => m.Send(It.IsAny<TestQuery>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(excelData);

//        // Act
//        var result = await _controller.TestExcelView(query);

//        // Assert
//        var fileResult = Assert.IsType<FileContentResult>(result);
//        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
//        Assert.Equal(excelData, fileResult.FileContents);

//        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    public async Task ExcelView_WithExcelFileViewModel_ShouldReturnFileContentResult()
//    {
//        // Arrange
//        var query = new TestQuery();
//        var excelFileViewModel = new ExcelFileViewModel("test.xlsx", new byte[] { 1, 2, 3 });

//        _mediatorMock.Setup(m => m.Send(It.IsAny<TestQuery>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(excelFileViewModel);

//        // Act
//        var result = await _controller.TestExcelView(query);

//        // Assert
//        var fileResult = Assert.IsType<FileContentResult>(result);
//        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
//        Assert.Equal(excelFileViewModel.FileContents, fileResult.FileContents);
//        Assert.Equal(excelFileViewModel.FileName, fileResult.FileDownloadName);

//        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
//    }
//}