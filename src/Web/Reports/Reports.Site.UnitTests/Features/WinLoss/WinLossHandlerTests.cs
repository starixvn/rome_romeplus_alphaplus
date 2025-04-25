using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.WinLoss;
using Microsoft.Extensions.Options;

namespace Reports.Site.UnitTests.Features.WinLoss;

public class WinLossHandlerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IOptions<AppSettings>> _optionsMock;
    private readonly Mock<IWinLossService> _winLossServiceMock;
    private readonly Mock<IWinLossWorkbook> _workbookMock;
    private readonly WinLossHandler _handler;

    public WinLossHandlerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _optionsMock = new Mock<IOptions<AppSettings>>();
        _winLossServiceMock = new Mock<IWinLossService>();
        _workbookMock = new Mock<IWinLossWorkbook>();

        _optionsMock.Setup(o => o.Value).Returns(new AppSettings());

        _handler = new WinLossHandler(
            _productServiceMock.Object,
            _optionsMock.Object,
            _winLossServiceMock.Object,
            _workbookMock.Object);
    }

    [Fact]
    public async Task Handle_WinLossQuery_ShouldReturnViewModel()
    {
        // Arrange
        var query = new WinLossQuery
        {
            ViewerId = "user1",
            ViewerLevel = 1,
            FromDate = DateTime.Now.AddDays(-7),
            ToDate = DateTime.Now,
            UserSelectedProductIds = "1,2,3",
            UserSelectedResourceIds = "100,200"
        };

        var productViewModel = new ProductViewModel();
        var reportData = new List<WinLossModel>();
        var summary = new WinLossSummaryModel();

        _productServiceMock.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryParam>()))
            .ReturnsAsync((new List<ProductModel>(), "productDetailXml"));

        _winLossServiceMock.Setup(s => s.GetWinLoss(It.IsAny<WinLossParam>()))
            .ReturnsAsync((reportData, summary));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<WinLossViewModel>(result);

        _winLossServiceMock.Verify(
            s => s.GetWinLoss(It.Is<WinLossParam>(p =>
                p.ViewerId == query.ViewerId &&
                p.Level == query.ViewerLevel.Value)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WinLossExcelQuery_ShouldReturnExcelFile()
    {
        // Arrange
        var query = new WinLossExcelQuery
        {
            ViewerId = "user1",
            ViewerLevel = 1,
            FromDate = DateTime.Now.AddDays(-7),
            ToDate = DateTime.Now,
            UserSelectedProductIds = "1,2,3",
            UserSelectedResourceIds = "100,200"
        };

        var validProductIds = "1,2,3";
        var productDetailXml = "productDetailXml";
        var productNames = "Product Names";
        var reportData = new List<WinLossModel>();
        var summary = new WinLossSummaryModel();
        var excelFile = new ExcelFileViewModel("filename", new byte[] { 1, 2, 3 });

        _productServiceMock.Setup(s => s.FilterProductAndProductDetailIdsForExcel(
            It.IsAny<ProductQueryParam>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((validProductIds, productDetailXml, productNames));

        _winLossServiceMock.Setup(s => s.GetWinLoss(It.IsAny<WinLossParam>()))
            .ReturnsAsync((reportData, summary));

        _workbookMock.Setup(w => w.BuildExcelFile(
            It.IsAny<WinLossExcelQuery>(),
            It.IsAny<IEnumerable<WinLossModel>>(),
            It.IsAny<WinLossSummaryModel>(),
            It.IsAny<string>()))
            .Returns(excelFile);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(excelFile.FileContents, result.FileContents);

        _workbookMock.Verify(
            w => w.BuildExcelFile(
                query,
                It.IsAny<IEnumerable<WinLossModel>>(),
                It.IsAny<WinLossSummaryModel>(),
                productNames),
            Times.Once);
    }
}