//using AlphaPlus.Core.Models;
//using AlphaPlus.Reports.Features.Products;
//using AlphaPlus.Reports.Features.Shared.Models;
//using AlphaPlus.Reports.Features.WinLossByProduct;
//using AlphaPlus.Reports.Site.Features.WinLossByProduct;
//using Microsoft.Extensions.Options;

//namespace Reports.Site.UnitTests.Features.WinLossByProduct;

//public class WinLossByProductHandlerTests
//{
//    private readonly Mock<IProductService> _productServiceMock;
//    private readonly Mock<IOptions<AppSettings>> _optionsMock;
//    private readonly Mock<IWinLossByProductService> _winLossByProductServiceMock;
//    private readonly Mock<IWinLossByProductWorkbook> _workbookMock;
//    private readonly WinLossByProductHandler _handler;

//    public WinLossByProductHandlerTests()
//    {
//        _productServiceMock = new Mock<IProductService>();
//        _optionsMock = new Mock<IOptions<AppSettings>>();
//        _winLossByProductServiceMock = new Mock<IWinLossByProductService>();
//        _workbookMock = new Mock<IWinLossByProductWorkbook>();

//        _optionsMock.Setup(o => o.Value).Returns(new AppSettings());

//        _handler = new WinLossByProductHandler(
//            _productServiceMock.Object,
//            _optionsMock.Object,
//            _winLossByProductServiceMock.Object,
//            _workbookMock.Object);
//    }

//    [Fact]
//    public async Task Handle_WinLossByProductQuery_ShouldReturnViewModel()
//    {
//        // Arrange
//        var query = new WinLossByProductQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = 1,
//            FromDate = DateTime.Now.AddDays(-7),
//            ToDate = DateTime.Now,
//            UserSelectedProductIds = "1,2,3",
//            UserSelectedResourceIds = "100,200"
//        };

//        var productViewModel = new ProductViewModel();
//        var reportData = new List<WinLossByProductModel>();
//        var total = new WinLossByBetTypeModel();

//        _productServiceMock.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryParam>()))
//            .ReturnsAsync((new List<ProductModel>(), "productDetailXml"));

//        _winLossByProductServiceMock.Setup(s => s.GetWinLossByProduct(It.IsAny<WinLossByProductParam>()))
//            .ReturnsAsync((reportData, total));

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        Assert.NotNull(result);
//        Assert.IsType<WinLossByProductViewModel>(result);

//        _winLossByProductServiceMock.Verify(
//            s => s.GetWinLossByProduct(It.Is<WinLossByProductParam>(p =>
//                p.ViewerId == query.ViewerId &&
//                p.Level == query.ViewerLevel.Value)),
//            Times.Once);
//    }

//    [Fact]
//    public async Task Handle_WinLossByProductExcelQuery_ShouldReturnExcelFile()
//    {
//        // Arrange
//        var query = new WinLossByProductExcelQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = 1,
//            FromDate = DateTime.Now.AddDays(-7),
//            ToDate = DateTime.Now,
//            UserSelectedProductIds = "1,2,3",
//            UserSelectedResourceIds = "100,200"
//        };

//        var validProductIds = "1,2,3";
//        var productDetailXml = "productDetailXml";
//        var productNames = new Dictionary<string, string> { { "1", "Product 1" } };
//        var reportData = new List<WinLossByProductModel>();
//        var total = new WinLossByBetTypeModel();
//        var excelFile = new ExcelFileViewModel("filename", new byte[] { 1, 2, 3 });

//        _productServiceMock.Setup(s => s.FilterProductAndProductDetailIdsForExcel(
//            It.IsAny<ProductQueryParam>(), It.IsAny<string>(), It.IsAny<string>()))
//            .ReturnsAsync((validProductIds, productDetailXml, productNames));

//        _winLossByProductServiceMock.Setup(s => s.GetWinLossByProduct(It.IsAny<WinLossByProductParam>()))
//            .ReturnsAsync((reportData, total));

//        _workbookMock.Setup(w => w.BuildExcelFile(
//            It.IsAny<WinLossByProductExcelQuery>(),
//            It.IsAny<IEnumerable<WinLossByProductModel>>(),
//            It.IsAny<WinLossByBetTypeModel>(),
//            It.IsAny<Dictionary<string, string>>()))
//            .Returns(excelFile);

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(excelFile, result);
//        Assert.Equal(excelFile.FileContents, result.FileContents);

//        _workbookMock.Verify(
//            w => w.BuildExcelFile(
//                It.IsAny<WinLossByProductExcelQuery>(),
//                It.IsAny<IEnumerable<WinLossByProductModel>>(),
//                It.IsAny<WinLossByBetTypeModel>(),
//                It.IsAny<Dictionary<string, string>>()),
//            Times.Once);
//    }
//}