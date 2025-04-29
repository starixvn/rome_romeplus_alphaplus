using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared;

[Trait("Category", "Shared")]
public class BaseHandlerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IOptions<AppSettings>> _mockOptions;
    private readonly BaseHandler _baseHandler;
    private readonly AppSettings _appSettings;

    public BaseHandlerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _appSettings = new AppSettings();
        _mockOptions = new Mock<IOptions<AppSettings>>();
        _mockOptions.Setup(o => o.Value).Returns(_appSettings);
        _baseHandler = new BaseHandler(_mockProductService.Object, _mockOptions.Object);
    }

    [Fact]
    public async Task BuildProductViewModel_With_ResourceIds_Should_Return_Correct_ViewModel()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var userSelectedProductIds = "1,2";
        var userSelectedResourceIds = "101,102";

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Type = 1, TypeName = "Type1" },
            new Product { Id = 2, Name = "Product2", Type = 1, TypeName = "Type1" },
            new Product { Id = 3, Name = "Product3", Type = 2, TypeName = "Type2" }
        };

        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101, DisplayName = "Resource1" },
            new ProductDetail { ProductId = 2, ResourceId = 102, DisplayName = "Resource2" }
        };

        _mockProductService.Setup(s => s.GetProducts(It.IsAny<GetProductsParam>())).ReturnsAsync(products);
        _mockProductService.Setup(s => s.GetProductDetails(It.IsAny<int>(), It.IsAny<IEnumerable<Product>>())).ReturnsAsync(productDetails);

        // Act
        var (viewModel, productDetailXml) = await _baseHandler.BuildProductViewModel(message, userSelectedProductIds, userSelectedResourceIds);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(productDetailXml);
        Assert.Equal("1,2", viewModel.UserSelectedProductIds);
        Assert.Equal("101,102", viewModel.UserSelectedResourceIds);
        Assert.Equal("en-US", viewModel.Language);
        Assert.NotEmpty(viewModel.Products);
        Assert.NotEmpty(viewModel.ProductDetails);
        Assert.Contains("<ROOT>", productDetailXml);
        Assert.Contains("</ROOT>", productDetailXml);
    }

    [Fact]
    public async Task BuildProductViewModel_With_All_Products_Selected_Should_Return_Correct_ViewModel()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var userSelectedProductIds = "*";
        var userSelectedResourceIds = "101,102";

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Type = 1, TypeName = "Type1" },
            new Product { Id = 2, Name = "Product2", Type = 1, TypeName = "Type1" },
            new Product { Id = 3, Name = "Product3", Type = 2, TypeName = "Type2" }
        };

        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101, DisplayName = "Resource1" },
            new ProductDetail { ProductId = 2, ResourceId = 102, DisplayName = "Resource2" }
        };

        _mockProductService.Setup(s => s.GetProducts(It.IsAny<GetProductsParam>())).ReturnsAsync(products);
        _mockProductService.Setup(s => s.GetProductDetails(It.IsAny<int>(), It.IsAny<IEnumerable<Product>>())).ReturnsAsync(productDetails);

        // Act
        var (viewModel, productDetailXml) = await _baseHandler.BuildProductViewModel(message, userSelectedProductIds, userSelectedResourceIds);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(productDetailXml);
        Assert.Equal("1,2,3", viewModel.UserSelectedProductIds);
        Assert.Equal("101,102", viewModel.UserSelectedResourceIds);
        Assert.NotEmpty(viewModel.Products);
        Assert.All(viewModel.Products, group => Assert.True(group.Checked));
    }

    [Fact]
    public async Task BuildProductViewModel_With_All_Resources_Selected_Should_Return_Correct_ViewModel()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var userSelectedProductIds = "1,2";
        var userSelectedResourceIds = "*";

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Type = 1, TypeName = "Type1" },
            new Product { Id = 2, Name = "Product2", Type = 1, TypeName = "Type1" }
        };

        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101, DisplayName = "Resource1" },
            new ProductDetail { ProductId = 1, ResourceId = 102, DisplayName = "Resource2" },
            new ProductDetail { ProductId = 2, ResourceId = 103, DisplayName = "Resource3" }
        };

        _mockProductService.Setup(s => s.GetProducts(It.IsAny<GetProductsParam>())).ReturnsAsync(products);
        _mockProductService.Setup(s => s.GetProductDetails(It.IsAny<int>(), It.IsAny<IEnumerable<Product>>())).ReturnsAsync(productDetails);

        // Act
        var (viewModel, productDetailXml) = await _baseHandler.BuildProductViewModel(message, userSelectedProductIds, userSelectedResourceIds);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(productDetailXml);
        Assert.Equal("1,2", viewModel.UserSelectedProductIds);
        Assert.Equal("101,102,103", viewModel.UserSelectedResourceIds);
        Assert.All(viewModel.ProductDetails, detail => Assert.True(detail.Checked));
    }

    [Fact]
    public async Task BuildProductViewModel_Without_ResourceIds_Should_Return_Correct_ViewModel()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var userSelectedProductIds = "1,2";

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Type = 0, TypeName = "" },
            new Product { Id = 2, Name = "Product2", Type = 0, TypeName = "" },
            new Product { Id = 3, Name = "Product3", Type = 0, TypeName = "" }
        };

        _mockProductService.Setup(s => s.GetProducts(It.IsAny<GetProductsParam>())).ReturnsAsync(products);

        // Act
        var viewModel = await _baseHandler.BuildProductViewModel(message, userSelectedProductIds);

        // Assert
        Assert.NotNull(viewModel);
        Assert.Equal("1,2", viewModel.UserSelectedProductIds);
        Assert.Equal("en-US", viewModel.Language);
        Assert.NotEmpty(viewModel.Products);
        Assert.Equal(3, viewModel.Products.Count());

        // Products with Type 0 should be added directly to productGroups
        var product1Group = viewModel.Products.FirstOrDefault(g => g.Id == 1);
        var product2Group = viewModel.Products.FirstOrDefault(g => g.Id == 2);
        var product3Group = viewModel.Products.FirstOrDefault(g => g.Id == 3);

        Assert.NotNull(product1Group);
        Assert.NotNull(product2Group);
        Assert.NotNull(product3Group);
        Assert.True(product1Group.Checked);
        Assert.True(product2Group.Checked);
        Assert.False(product3Group.Checked);
    }

    [Fact]
    public async Task BuildProductViewModel_With_Custom_Products_Should_Use_Provided_Products()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var userSelectedProductIds = "1,2";
        var userSelectedResourceIds = "101";

        var customProducts = new List<Product>
        {
            new Product { Id = 1, Name = "CustomProduct1", Type = 1, TypeName = "CustomType" },
            new Product { Id = 2, Name = "CustomProduct2", Type = 1, TypeName = "CustomType" }
        };

        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101, DisplayName = "CustomResource" }
        };

        // GetProducts should not be called
        _mockProductService.Setup(s => s.GetProducts(It.IsAny<GetProductsParam>())).Throws(new System.Exception("This should not be called"));
        _mockProductService.Setup(s => s.GetProductDetails(It.IsAny<int>(), It.IsAny<IEnumerable<Product>>())).ReturnsAsync(productDetails);

        // Act
        var (viewModel, productDetailXml) = await _baseHandler.BuildProductViewModel(message, userSelectedProductIds, userSelectedResourceIds, customProducts);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(productDetailXml);
        Assert.Equal("1,2", viewModel.UserSelectedProductIds);
        Assert.Equal("101", viewModel.UserSelectedResourceIds);
        Assert.Single(viewModel.Products);

        // Check that the group has the custom type name
        var group = viewModel.Products.First();
        Assert.Equal("CustomType", group.Name);
        Assert.Equal(2, group.Products.Count());
    }
}