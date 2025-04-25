using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.WinLossAnalysisChart;
using Microsoft.Extensions.Options;

namespace Reports.Site.UnitTests.Features.WinLossAnalysisChart;

public class WinLossAnalysisChartHandlerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IOptions<AppSettings>> _optionsMock;
    private readonly Mock<IWinLossAnalysisService> _winLossAnalysisServiceMock;
    private readonly WinLossAnalysisChartHandler _handler;

    public WinLossAnalysisChartHandlerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _optionsMock = new Mock<IOptions<AppSettings>>();
        _winLossAnalysisServiceMock = new Mock<IWinLossAnalysisService>();

        _optionsMock.Setup(o => o.Value).Returns(new AppSettings());

        _handler = new WinLossAnalysisChartHandler(
            _productServiceMock.Object,
            _optionsMock.Object,
            _winLossAnalysisServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnChartViewModel()
    {
        // Arrange
        var query = new WinLossAnalysisChartQuery
        {
            ViewerId = "user1",
            ViewerLevel = 1,
            FromDate = DateTime.Now.AddDays(-7),
            ToDate = DateTime.Now,
            UserSelectedProductIds = "1,2,3",
            UserSelectedResourceIds = "100,200",
            TopN = 10
        };

        var reportData = new List<WinLossAnalysisModel>
        {
            new WinLossAnalysisModel
            {
                CustomerId = "cust1",
                CustomerName = "Customer 1",
                MemberCount = 5,
                Turnover = 1000,
                WinLoss = 300,
                NetWL = 275
            },
            new WinLossAnalysisModel
            {
                CustomerId = "cust2",
                CustomerName = "Customer 2",
                MemberCount = 3,
                Turnover = 800,
                WinLoss = 200,
                NetWL = 180
            }
        };

        var summary = new WinLossSummaryModel
        {
            MemberCount = 8,
            Turnover = 1800,
            WinLoss = 500,
            NetWL = 455
        };

        _productServiceMock.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryParam>()))
            .ReturnsAsync((new List<ProductModel>(), "productDetailXml"));

        _winLossAnalysisServiceMock.Setup(s => s.GetWinLossAnalysis(It.IsAny<WinLossAnalysisParam>()))
            .ReturnsAsync((reportData, summary));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<WinLossAnalysisChartViewModel>(result);
        Assert.Equal(query.FromDate, result.FromDate);
        Assert.Equal(query.ToDate, result.ToDate);
        Assert.NotEmpty(result.ChartData);

        _winLossAnalysisServiceMock.Verify(
            s => s.GetWinLossAnalysis(It.Is<WinLossAnalysisParam>(p =>
                p.ViewerId == query.ViewerId &&
                p.Level == query.ViewerLevel.Value &&
                p.TopN == query.TopN)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoData_ShouldReturnEmptyChartViewModel()
    {
        // Arrange
        var query = new WinLossAnalysisChartQuery
        {
            ViewerId = "user1",
            ViewerLevel = 1,
            FromDate = DateTime.Now.AddDays(-7),
            ToDate = DateTime.Now,
            UserSelectedProductIds = "1,2,3",
            UserSelectedResourceIds = "100,200",
            TopN = 10
        };

        var reportData = new List<WinLossAnalysisModel>();
        var summary = new WinLossSummaryModel();

        _productServiceMock.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryParam>()))
            .ReturnsAsync((new List<ProductModel>(), "productDetailXml"));

        _winLossAnalysisServiceMock.Setup(s => s.GetWinLossAnalysis(It.IsAny<WinLossAnalysisParam>()))
            .ReturnsAsync((reportData, summary));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<WinLossAnalysisChartViewModel>(result);
        Assert.Equal(query.FromDate, result.FromDate);
        Assert.Equal(query.ToDate, result.ToDate);
        Assert.Empty(result.ChartData);
    }
}