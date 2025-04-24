using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.UnitTests.Features.Products;

[Trait("Category", "Products")]
public class ProductServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IMemoryCache> cacheMock;
    private readonly Mock<ICacheRefService> cacheRefServiceMock;
    private readonly Mock<IOptions<AppSettings>> optionsMock;
    private readonly Mock<ICacheEntry> cacheEntryMock;
    private readonly Func<int, bool> canViewProductMock;
    private readonly ProductService service;
    private readonly string siteName = "TestSite";

    public ProductServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        cacheMock = new Mock<IMemoryCache>();
        cacheRefServiceMock = new Mock<ICacheRefService>();
        optionsMock = new Mock<IOptions<AppSettings>>();
        cacheEntryMock = new Mock<ICacheEntry>();
        canViewProductMock = id => true;

        optionsMock.Setup(o => o.Value).Returns(new AppSettings
        {
            SiteLabel = "XYZ",
            ProductOfferProductDetail = new HashSet<int> { 1, 2, 3 }
        });

        cacheMock
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntryMock.Object);

        service = new ProductService(
            repositoryMock.Object,
            cacheMock.Object,
            canViewProductMock,
            cacheRefServiceMock.Object,
            optionsMock.Object);
    }

    [Fact]
    public void ConcatProductNames_ShouldReturnCommaSeparatedNames()
    {
        // Arrange
        var productIds = new List<int> { 1, 2, 3 };
        cacheRefServiceMock.Setup(x => x.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(x => x.GetProductName(2)).Returns("Product2");
        cacheRefServiceMock.Setup(x => x.GetProductName(3)).Returns("Product3");

        // Act
        var result = service.ConcatProductNames(productIds);

        // Assert
        Assert.Equal("Product1, Product2, Product3", result);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnFromCache_WhenCachedValueExists()
    {
        // Arrange
        var param = new GetProductsParam(
            SiteName: siteName,
            CurrencyId: 1,
            SettingStatus: 0, // Assuming default value for sportId parameter
            Language: "en",
            MinReportDate: null // Assuming MinReportDate is optional
        );

        var expectedProducts = new List<Product>
        {
            new() { Id = 1, Name = "Product1" },
            new() { Id = 2, Name = "Product2" }
        };

        object cachedValue = expectedProducts;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(true);

        // Act
        var result = await service.GetProducts(param);

        // Assert
        Assert.Equal(expectedProducts, result);
        repositoryMock.Verify(
            x => x.FetchAsync<Product>(It.IsAny<GetProductsCriteria>(), string.Empty),
            Times.Never);
    }

    [Fact]
    public async Task GetProducts_ShouldFetchFromRepository_WhenCachedValueDoesNotExist()
    {
        // Arrange
        var param = new GetProductsParam(
            SiteName: siteName,
            CurrencyId: 1,
            SettingStatus: 0, // Assuming default value for sportId parameter
            Language: "en",
            MinReportDate: null // Assuming MinReportDate is optional
        );

        var products = new List<Product>
        {
            new Product { Id = 1, IsHide = false },
            new Product { Id = 2, IsHide = false },
            new Product { Id = 3, IsHide = true }
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.FetchAsync<Product>(It.IsAny<GetProductsCriteria>(), string.Empty))
            .ReturnsAsync(products);

        cacheRefServiceMock.Setup(x => x.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(x => x.GetProductName(2)).Returns("Product2");

        // Act
        var result = await service.GetProducts(param);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, p => p.Id == 1);
        Assert.Contains(resultList, p => p.Id == 2);
        Assert.DoesNotContain(resultList, p => p.Id == 3); // Hidden products should be filtered out
    }

    [Fact]
    public async Task GetProducts_ShouldFilterByEndOfSupportDate_WhenMinReportDateIsProvided()
    {
        // Arrange
        var today = DateTime.Today;
        var param = new GetProductsParam(
            SiteName: siteName,
            CurrencyId: 1,
            SettingStatus: 0, // Assuming default value for sportId parameter
            Language: "en",
            MinReportDate: today
        );

        var products = new List<Product>
        {
            new Product { Id = 1, IsHide = false, EndOfSupportDate = today.AddDays(1) },
            new Product { Id = 2, IsHide = false, EndOfSupportDate = today.AddDays(-1) },
            new Product { Id = 3, IsHide = false, EndOfSupportDate = null }
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.FetchAsync<Product>(It.IsAny<GetProductsCriteria>(), string.Empty))
            .ReturnsAsync(products);

        // Act
        var result = await service.GetProducts(param);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, p => p.Id == 1);
        Assert.Contains(resultList, p => p.Id == 3);
        Assert.DoesNotContain(resultList, p => p.Id == 2); // Expired product should be filtered out
    }

    [Fact]
    public async Task GetProducts_ShouldSetNameAndTypeName()
    {
        // Arrange
        var param = new GetProductsParam(
            SiteName: siteName,
            CurrencyId: 1,
            SettingStatus: 0, // Assuming default value for sportId parameter
            Language: "en",
            MinReportDate: null // Assuming MinReportDate is optional
        );

        var products = new List<Product>
        {
            new Product { Id = 1, IsHide = false, Type = 10 },
            new Product { Id = 2, IsHide = false, Type = 0 }
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.FetchAsync<Product>(It.IsAny<GetProductsCriteria>(), string.Empty))
            .ReturnsAsync(products);

        cacheRefServiceMock.Setup(x => x.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(x => x.GetProductName(2)).Returns("Product2");
        cacheRefServiceMock.Setup(x => x.GetProductName(10)).Returns("Type10");

        // Act
        var result = await service.GetProducts(param);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("Product1", resultList[0].Name);
        Assert.Equal("Type10", resultList[0].TypeName);
        Assert.Equal("Product2", resultList[1].Name);
        Assert.Equal(string.Empty, resultList[1].TypeName);
    }

    [Fact]
    public async Task GetProductDetails_ShouldReturnFromCache_WhenCachedValueExists()
    {
        // Arrange
        var custId = 1;
        var allOfferProducts = new List<Product>
        {
            new Product { Id = 1 },
            new Product { Id = 2 }
        };

        var expectedProductDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 10, DisplayName = "Detail1" },
            new ProductDetail { ProductId = 2, ResourceId = 20, DisplayName = "Detail2" }
        };

        object cachedValue = expectedProductDetails;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(true);

        // Act
        var result = await service.GetProductDetails(custId, allOfferProducts);

        // Assert
        Assert.Equal(expectedProductDetails, result);
        repositoryMock.Verify(
            x => x.FetchAsync<ProductDetail>(It.IsAny<GetProductsDetailCriteria>(), string.Empty),
            Times.Never);
    }

    [Fact]
    public async Task GetProductDetails_ShouldFetchFromRepository_WhenCachedValueDoesNotExist()
    {
        // Arrange
        var custId = 1;
        var allOfferProducts = new List<Product>
        {
            new Product { Id = 1 },
            new Product { Id = 2 },
            new Product { Id = 4 }
        };

        var allProductDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 10, Order = 1 },
            new ProductDetail { ProductId = 2, ResourceId = 20, Order = 1 },
            new ProductDetail { ProductId = 3, ResourceId = 30, Order = 1 }
        };

        object cachedValue = null;
        object allDetailsCachedValue = null;

        cacheMock
            .Setup(x => x.TryGetValue(It.Is<string>(s => s.Contains(custId.ToString())), out cachedValue))
            .Returns(false);

        cacheMock
            .Setup(x => x.TryGetValue("AlphaPlus.Reports.ProductDetails", out allDetailsCachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.FetchAsync<ProductDetail>(It.IsAny<GetProductsDetailCriteria>(), string.Empty))
            .ReturnsAsync(allProductDetails);

        cacheRefServiceMock.Setup(x => x.GetProductDetailName(10)).Returns("Detail1");
        cacheRefServiceMock.Setup(x => x.GetProductDetailName(20)).Returns("Detail2");

        // Act
        var result = await service.GetProductDetails(custId, allOfferProducts);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, p => p.ProductId == 1 && p.ResourceId == 10 && p.DisplayName == "Detail1");
        Assert.Contains(resultList, p => p.ProductId == 2 && p.ResourceId == 20 && p.DisplayName == "Detail2");
        Assert.DoesNotContain(resultList, p => p.ProductId == 3); // Not in offer products
    }

    [Fact]
    public async Task GetProductsNeedDelayTimeToSummarizeData_ShouldFilterByEnabledJackpotProductIds()
    {
        // Arrange
        var param = new GetProductsParam(
            SiteName: siteName,
            CurrencyId: 1,
            SettingStatus: 0, // Assuming default value for sportId parameter
            Language: "en",
            MinReportDate: null // Assuming MinReportDate is optional
        );

        var productsNeedDelay = new List<ProductsByRefTable>
        {
            new ProductsByRefTable { ProductId = 1 },
            new ProductsByRefTable { ProductId = 2 },
            new ProductsByRefTable { ProductId = 3 }
        };

        var enabledJackpotProductIds = new List<int> { 1, 3 };

        var products = new List<Product>
        {
            new Product { Id = 1, IsHide = false },
            new Product { Id = 2, IsHide = false },
            new Product { Id = 3, IsHide = false }
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.FetchAsync<ProductsByRefTable>(It.IsAny<GetProductsByRefTableCriteria>(), string.Empty))
            .ReturnsAsync(productsNeedDelay);

        repositoryMock
            .Setup(x => x.FetchAsync<Product>(It.IsAny<GetProductsCriteria>(), string.Empty))
            .ReturnsAsync(products);

        cacheRefServiceMock.Setup(x => x.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(x => x.GetProductName(3)).Returns("Product3");

        // Act
        var result = await service.GetProductsNeedDelayTimeToSummarizeData(param, enabledJackpotProductIds);

        // Assert
        Assert.Equal("Product1, Product3", result);
    }

    [Fact]
    public void GetProductNamesExportExcel_ShouldIncludeProductDetailNames_WhenPartiallySelected()
    {
        // Arrange
        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101 },
            new ProductDetail { ProductId = 1, ResourceId = 102 },
            new ProductDetail { ProductId = 2, ResourceId = 201 },
            new ProductDetail { ProductId = 2, ResourceId = 202 }
        };

        var selectedProductIds = new List<int> { 1, 2 };
        var selectedResourceIds = new List<int> { 101, 201 }; // Partial selection

        cacheRefServiceMock.Setup(x => x.GetProductDetailName(101)).Returns("Detail101");
        cacheRefServiceMock.Setup(x => x.GetProductDetailName(201)).Returns("Detail201");
        cacheRefServiceMock.Setup(x => x.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(x => x.GetProductName(2)).Returns("Product2");

        // Act
        var result = service.GetProductNamesExportExcel(productDetails, selectedProductIds, selectedResourceIds);

        // Assert
        Assert.Equal("Detail101, Detail201", result);
    }

    [Fact]
    public void GetProductNamesExportExcel_ShouldUseProductName_WhenAllResourcesSelected()
    {
        // Arrange
        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101 },
            new ProductDetail { ProductId = 2, ResourceId = 201 }
        };

        var selectedProductIds = new List<int> { 1, 2 };
        var selectedResourceIds = new List<int> { 101, 201 }; // All resources selected

        cacheRefServiceMock.Setup(x => x.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(x => x.GetProductName(2)).Returns("Product2");

        // Act
        var result = service.GetProductNamesExportExcel(productDetails, selectedProductIds, selectedResourceIds);

        // Assert
        Assert.Equal("Product1, Product2", result);
    }

    [Fact]
    public void GetProductNamesExportExcel_ShouldReturnEmptyString_WhenSelectedProductIdsIsNull()
    {
        // Arrange
        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101 }
        };

        List<int> selectedProductIds = null;
        var selectedResourceIds = new List<int> { 101 };

        // Act
        var result = service.GetProductNamesExportExcel(productDetails, selectedProductIds, selectedResourceIds);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}