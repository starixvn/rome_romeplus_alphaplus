//using AlphaPlus.Core.Models;
//using AlphaPlus.Reports.Features.Products;
//using AlphaPlus.Reports.Features.Shared.Models;
//using AlphaPlus.Reports.Features.WinLossAnalysis;
//using AlphaPlus.Reports.Site.Features.WinLossAnalysis;
//using Microsoft.Extensions.Options;

//namespace Reports.Site.UnitTests.Features.WinLossAnalysis;

//public class WinLossAnalysisHandlerTests
//{
//    private readonly Mock<IProductService> _productServiceMock;
//    private readonly Mock<IOptions<AppSettings>> _optionsMock;
//    private readonly Mock<IWinLossAnalysisService> _winLossAnalysisServiceMock;
//    private readonly Mock<IWinLossAnalysisWorkbook> _workbookMock;
//    private readonly WinLossAnalysisHandler _handler;

//    public WinLossAnalysisHandlerTests()
//    {
//        _productServiceMock = new Mock<IProductService>();
//        _optionsMock = new Mock<IOptions<AppSettings>>();
//        _winLossAnalysisServiceMock = new Mock<IWinLossAnalysisService>();
//        _workbookMock = new Mock<IWinLossAnalysisWorkbook>();

//        _optionsMock.Setup(o => o.Value).Returns(new AppSettings());

//        _handler = new WinLossAnalysisHandler(
//            _productServiceMock.Object,
//            _optionsMock.Object,
//            _winLossAnalysisServiceMock.Object,
//            _workbookMock.Object);
//    }

//    [Fact]
//    public async Task Handle_WinLossAnalysisQuery_ShouldReturnViewModel()
//    {
//        // Arrange
//        var query = new WinLossAnalysisQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = 1,
//            FromDate = DateTime.Now.AddDays(-7),
//            ToDate = DateTime.Now,
//            UserSelectedProductIds = "1,2,3",
//            UserSelectedResourceIds = "100,200",
//            TopN = 10
//        };

//        var productViewModel = new ProductViewModel();
//        var reportData = new List<WinLossAnalysisModel>();
//        var summary = new WinLossSummaryModel();

//        _productServiceMock.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryParam>()))
//            .ReturnsAsync((new List<ProductModel>(), "productDetailXml"));

//        _winLossAnalysisServiceMock.Setup(s => s.GetWinLossAnalysis(It.IsAny<WinLossAnalysisParam>()))
//            .ReturnsAsync((reportData, summary));

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        Assert.NotNull(result);
//        Assert.IsType<WinLossAnalysisViewModel>(result);

//        _winLossAnalysisServiceMock.Verify(
//            s => s.GetWinLossAnalysis(It.Is<WinLossAnalysisParam>(p =>
//                p.ViewerId == query.ViewerId &&
//                p.Level == query.ViewerLevel.Value &&
//                p.TopN == query.TopN)),
//            Times.Once);
//    }

//    [Fact]
//    public async Task Handle_WinLossAnalysisExcelQuery_ShouldReturnExcelFile()
//    {
//        // Arrange
//        var query = new WinLossAnalysisExcelQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = 1,
//            FromDate = DateTime.Now.AddDays(-7),
//            ToDate = DateTime.Now,
//            UserSelectedProductIds = "1,2,3",
//            UserSelectedResourceIds = "100,200",
//            TopN = 10
//        };

//        var validProductIds = "1,2,3";
//        var productDetailXml = "productDetailXml";
//        var productNames = "Product Names";
//        var reportData = new List<WinLossAnalysisModel>();
//        var summary = new WinLossSummaryModel();
//        var excelFile = new ExcelFileViewModel("filename", new byte[] { 1, 2, 3 });

//        _productServiceMock.Setup(s => s.FilterProductAndProductDetailIdsForExcel(
//            It.IsAny<ProductQueryParam>(), It.IsAny<string>(), It.IsAny<string>()))
//            .ReturnsAsync((validProductIds, productDetailXml, productNames));

//        _winLossAnalysisServiceMock.Setup(s => s.GetWinLossAnalysis(It.IsAny<WinLossAnalysisParam>()))
//            .ReturnsAsync((reportData, summary));

//        _workbookMock.Setup(w => w.BuildExcelFile(
//            It.IsAny<WinLossAnalysisExcelQuery>(),
//            It.IsAny<IEnumerable<WinLossAnalysisModel>>(),
//            It.IsAny<WinLossSummaryModel>(),
//            It.IsAny<string>()))
//            .Returns(excelFile);

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(excelFile.FileContents, result.FileContents);

//        _workbookMock.Verify(
//            w => w.BuildExcelFile(
//                query,
//                It.IsAny<IEnumerable<WinLossAnalysisModel>>(),
//                It.IsAny<WinLossSummaryModel>(),
//                productNames),
//            Times.Once);
//    }
//}