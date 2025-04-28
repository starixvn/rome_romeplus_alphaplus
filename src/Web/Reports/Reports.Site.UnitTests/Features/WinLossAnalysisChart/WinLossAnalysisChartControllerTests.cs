//using AlphaPlus.Reports.Site.Features.WinLossAnalysisChart;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Reports.Site.UnitTests.Features.WinLossAnalysisChart;

//public class WinLossAnalysisChartControllerTests
//{
//    private readonly Mock<IMediator> _mediatorMock;
//    private readonly WinLossAnalysisChartController _controller;

//    public WinLossAnalysisChartControllerTests()
//    {
//        _mediatorMock = new Mock<IMediator>();
//        _controller = new WinLossAnalysisChartController(_mediatorMock.Object);
//    }

//    [Fact]
//    public async Task Index_ShouldReturnViewResult()
//    {
//        // Arrange
//        var query = new WinLossAnalysisChartQuery();
//        var viewModel = new WinLossAnalysisChartViewModel();

//        _mediatorMock.Setup(m => m.Send(It.IsAny<WinLossAnalysisChartQuery>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(viewModel);

//        // Act
//        var result = await _controller.Index(query);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.Equal(viewModel, viewResult.Model);

//        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
//    }
//}