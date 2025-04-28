//using AlphaPlus.Core.Models;
//using AlphaPlus.Reports.Features.Products;
//using AlphaPlus.Reports.Features.Shared.Models;
//using AlphaPlus.Reports.Site.Features.Shared;
//using AlphaPlus.Reports.Site.Features.Shared.Requests;
//using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
//using Microsoft.Extensions.Options;

//namespace Reports.Site.UnitTests.Features.Shared;

//public class BaseHandlerTests
//{
//    private class TestBaseHandler : BaseHandler
//    {
//        public TestBaseHandler(
//            IProductService productService,
//            IOptions<AppSettings> options) : base(productService, options)
//        {
//        }

//        public Task<(ProductMultiSelectionViewModel, string)> PublicBuildProductViewModel(
//            BaseQuery message,
//            string userSelectedProductIds,
//            string userSelectedResourceIds) =>
//                BuildProductViewModel(message, userSelectedProductIds, userSelectedResourceIds);

//        public Task<(IEnumerable<int>, string, string)> PublicFilterProductAndProductDetailIdsForExcel(
//            BaseQuery message,
//            string userSelectedProductIds,
//            string userSelectedResourceIds) =>
//                FilterProductAndProductDetailIdsForExcel(message, userSelectedProductIds, userSelectedResourceIds);
//    }

//    private readonly Mock<IUserSession> _userSession;
//    private readonly Mock<IProductService> _productServiceMock;
//    private readonly Mock<IOptions<AppSettings>> _optionsMock;
//    private readonly TestBaseHandler _handler;

//    public BaseHandlerTests()
//    {
//        _userSession = new Mock<IUserSession>();
//        _productServiceMock = new Mock<IProductService>();
//        _optionsMock = new Mock<IOptions<AppSettings>>();

//        _optionsMock.Setup(o => o.Value).Returns(new AppSettings());

//        _handler = new TestBaseHandler(
//            _productServiceMock.Object,
//            _optionsMock.Object);
//    }

//    [Fact]
//    public async Task BuildProductViewModel_ShouldReturnProductViewModelAndXml()
//    {
//        // Arrange
//        _userSession.Setup(u => u.CustId).Returns(1);
//        _userSession.Setup(u => u.RoleId).Returns(4);
//        _userSession.Setup(u => u.Site).Returns("TestSite");
//        _userSession.Setup(u => u.CurrencyId).Returns(20);
//        _userSession.Setup(u => u.Language).Returns("en-US");
//        _userSession.Setup(u => u.GetProperty("CustomerDateLimit", DateTime.MinValue)).Returns(DateTime.MinValue);
//        var message = new BaseQuery(_userSession.Object);

//        var userSelectedProductIds = "1,2,3";
//        var userSelectedResourceIds = "100,200";
//        var products = new List<ProductModel>();
//        var productDetailXml = "<ProductDetails></ProductDetails>";

//        _productServiceMock.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryParam>()))
//            .ReturnsAsync((products, productDetailXml));

//        // Act
//        var result = await _handler.PublicBuildProductViewModel(
//            message, userSelectedProductIds, userSelectedResourceIds);

//        // Assert
//        Assert.NotNull(result.Item1);
//        Assert.Equal(productDetailXml, result.Item2);

//        _productServiceMock.Verify(
//            s => s.GetProductsAsync(It.Is<ProductQueryParam>(p =>
//                p.ViewerId == message.ViewerId &&
//                p.ViewerLevel == message.ViewerLevel)),
//            Times.Once);
//    }

//    [Fact]
//    public async Task FilterProductAndProductDetailIdsForExcel_ShouldReturnFilteredData()
//    {
//        // Arrange
//        var message = new ProductQueryParam
//        {
//            ViewerId = "user1",
//            ViewerLevel = 1
//        };

//        var userSelectedProductIds = "1,2,3";
//        var userSelectedResourceIds = "100,200";
//        var validProductIds = "1,2";
//        var productDetailXml = "<ProductDetails></ProductDetails>";
//        var productNames = new Dictionary<string, string> { { "1", "Product 1" } };

//        _productServiceMock.Setup(s => s.FilterProductAndProductDetailIdsForExcel(
//            It.IsAny<ProductQueryParam>(), It.IsAny<string>(), It.IsAny<string>()))
//            .ReturnsAsync((validProductIds, productDetailXml, productNames));

//        // Act
//        var result = await _handler.PublicFilterProductAndProductDetailIdsForExcel(
//            message, userSelectedProductIds, userSelectedResourceIds);

//        // Assert
//        Assert.Equal(validProductIds, result.Item1);
//        Assert.Equal(productDetailXml, result.Item2);
//        Assert.Equal(productNames, result.Item3);

//        _productServiceMock.Verify(
//            s => s.FilterProductAndProductDetailIdsForExcel(
//                It.Is<ProductQueryParam>(p => p.ViewerId == message.ViewerId),
//                userSelectedProductIds,
//                userSelectedResourceIds),
//            Times.Once);
//    }
//}