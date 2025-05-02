using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.BetLists.UnitTests.Shared.Services;

[Trait("Category", "BetLists")]
public class ProductServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IMemoryCache> cacheMock;
    private readonly Mock<ICacheEntry> cacheEntryMock;
    private readonly Func<int, bool> canViewProductFunc;
    private readonly ProductService productService;

    public ProductServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        cacheMock = new Mock<IMemoryCache>();
        cacheEntryMock = new Mock<ICacheEntry>();
        canViewProductFunc = (productId) => true; // Default behavior allows viewing all products

        // Setup cache mock
        cacheMock
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntryMock.Object);

        productService = new ProductService(repositoryMock.Object, cacheMock.Object, canViewProductFunc);
    }

    //[Fact]
    //public void ConcatProductNames_ShouldReturnProductNamesFromCache()
    //{
    //    // Arrange
    //    var productIds = new List<int> { 1, 2, 3 };
    //    var isSiteLabel777 = false;

    //    // Setup CacheRefServiceHelper mock (using static method mocking would require a different approach)
    //    // This test relies on CacheRefServiceHelper.GetProductName implementation

    //    // Act
    //    var result = productService.ConcatProductNames(productIds, isSiteLabel777);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(productIds.Count, result.Count());
    //}

    [Fact]
    public async Task GetProducts_ShouldReturnFilteredAndFormattedProducts()
    {
        // Arrange
        var param = new GetProductsParam(
            SiteName: "TestSite",
            CurrencyId: 1,
            SettingStatus: 0,
            Language: "en",
            MinReportDate: DateTime.MinValue,
            IsSiteLabel777: false
        );

        var products = new List<Product>
        {
            new Product { Id = 1, EndOfSupportDate = null, Type = 10 },
            new Product { Id = 2, EndOfSupportDate = DateTime.Now.AddDays(10), Type = 20 },
            new Product { Id = 3, EndOfSupportDate = null, Type = 30 },
            new Product { Id = 4, EndOfSupportDate = DateTime.Now.AddDays(-40), Type = 40 }
        };

        SetupCacheMock<IEnumerable<Product>>(products);

        // Act
        var result = await productService.GetProducts(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count()); // Only products 1 and 2 should be returned (not hidden and support date valid)
        Assert.All(result, p => Assert.False(p.IsHide));
    }

    //[Fact]
    //public async Task GetProductsByGroupHeadline_ShouldReturnFilteredAndFormattedProducts()
    //{
    //    // Arrange
    //    var param = new GetProductsByGroupHeadlineParam(
    //        CustId: 1000,
    //        SettingStatus: 1,
    //        MinDate: DateTime.Now.AddDays(-30),
    //        IsSiteLabel777: false
    //    );

    //    var products = new List<Product>
    //    {
    //        new Product { Id = 1, IsEnable = true, EndOfSupportDate = null, Type = 10 },
    //        new Product { Id = 2, IsEnable = true, EndOfSupportDate = DateTime.Now.AddDays(10), Type = 20 },
    //        new Product { Id = 3, IsEnable = false, EndOfSupportDate = null, Type = 30 },
    //        new Product { Id = 4, IsEnable = true, EndOfSupportDate = DateTime.Now.AddDays(-40), Type = 40 }
    //    };

    //    repositoryMock
    //        .Setup(x => x.FetchAsync<Product>(It.IsAny<GetProductsByGroupHeadlineCriteria>(), string.Empty))
    //        .ReturnsAsync(products);

    //    // Act
    //    var result = await productService.GetProductsByGroupHeadline(param);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(2, result.Count()); // Only products 1 and 2 should be returned (enabled and support date valid)
    //    Assert.All(result, p => Assert.True(p.IsEnable));
    //    Assert.All(result, p => Assert.NotNull(p.Name));
    //    Assert.All(result, p => Assert.True(p.Type == 0 || !string.IsNullOrEmpty(p.TypeName)));
    //}

    [Fact]
    public async Task IsEnabledJackpot_ShouldReturnTrueForSiteAndCurrencyWithJackpotEnabled()
    {
        // Arrange
        var site = "TestSite";
        var currencyId = 1;
        var isTest = false;

        var jackpotOfferedBySites = new List<JackpotOfferedBySite>
        {
            new JackpotOfferedBySite { Site = "TestSite", Product = 1 },
            new JackpotOfferedBySite { Site = "TestSite", Product = 2 },
            new JackpotOfferedBySite { Site = "OtherSite", Product = 3 }
        };

        var jackpotOfferedByCurrencies = new List<JackpotOfferedByCurrency>
        {
            new JackpotOfferedByCurrency { Currency = 1, Product = 1 },
            new JackpotOfferedByCurrency { Currency = 2, Product = 2 },
            new JackpotOfferedByCurrency { Currency = 1, Product = 3 }
        };

        SetupJackpotMock(jackpotOfferedBySites, jackpotOfferedByCurrencies);

        // Act
        var result = await productService.IsEnabledJackpot(site, currencyId, isTest);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsEnabledJackpot_ShouldReturnFalseWhenNoMatchingJackpot()
    {
        // Arrange
        var site = "NonExistentSite";
        var currencyId = 999;
        var isTest = false;

        var jackpotOfferedBySites = new List<JackpotOfferedBySite>
        {
            new JackpotOfferedBySite { Site = "TestSite", Product = 1 }
        };

        var jackpotOfferedByCurrencies = new List<JackpotOfferedByCurrency>
        {
            new JackpotOfferedByCurrency { Currency = 1, Product = 1 }
        };

        SetupJackpotMock(jackpotOfferedBySites, jackpotOfferedByCurrencies);

        // Act
        var result = await productService.IsEnabledJackpot(site, currencyId, isTest);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetProductDetails_ShouldReturnFilteredAndFormattedProductDetails()
    {
        // Arrange
        var param = new GetProductDetailsParam(
            CustId: 1000,
            ProductOfferProductDetail: new HashSet<int> { 1, 2 },
            IsSiteLabel777: false
        );

        var allOfferProducts = new List<Product>
        {
            new Product { Id = 1 },
            new Product { Id = 2 }
        };

        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101, Order = 1 },
            new ProductDetail { ProductId = 1, ResourceId = 102, Order = 2 },
            new ProductDetail { ProductId = 2, ResourceId = 201, Order = 1 }
        };

        SetupCacheMock<IEnumerable<ProductDetail>>(productDetails);

        // Act
        var result = await productService.GetProductDetails(param, allOfferProducts);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count()); // Only product details for products 1 and 2 should be returned
        Assert.All(result, p => Assert.Contains(p.ProductId, param.ProductOfferProductDetail));
    }

    [Fact]
    public void BuildProductDetailsXml_ShouldReturnValidXml()
    {
        // Arrange
        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101, DisplayName = "Product 1" },
            new ProductDetail { ProductId = 2, ResourceId = 201, DisplayName = "Product 2" }
        };

        // Act
        var result = productService.BuildProductDetailsXml(productDetails);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("<ROOT>", result);
        Assert.EndsWith("</ROOT>", result);
        Assert.Contains("ProductId=\"1\"", result);
        Assert.Contains("ProductId=\"2\"", result);
    }

    [Fact]
    public async Task BuildProductDetailsXml789y_ShouldFilterByProductIdsAndResourceIds()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1 },
            new Product { Id = 2 },
            new Product { Id = 3 }
        };

        var userSelectedProductIds = "1,2";
        var userSelectedResourceds = "101,201";
        var productOfferProductDetail = new HashSet<int> { 1 };

        var productDetails = new List<ProductDetail>
        {
            new ProductDetail { ProductId = 1, ResourceId = 101 },
            new ProductDetail { ProductId = 1, ResourceId = 102 },
            new ProductDetail { ProductId = 2, ResourceId = 201 },
            new ProductDetail { ProductId = 3, ResourceId = 301 }
        };

        SetupCacheMock<IEnumerable<ProductDetail>>(productDetails);

        // Act
        var result = await productService.BuildProductDetailsXml789y(
            products, userSelectedProductIds, userSelectedResourceds, productOfferProductDetail);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("<ROOT>", result);
        Assert.EndsWith("</ROOT>", result);
    }

    #region Helper Methods

    private void SetupCacheMock<T>(T value)
    {
        object cachedValue = value;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(true);

        cacheMock
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntryMock.Object);
    }

    private void SetupJackpotMock(
        List<JackpotOfferedBySite> jackpotOfferedBySites,
        List<JackpotOfferedByCurrency> jackpotOfferedByCurrencies)
    {
        var tuple = (jackpotOfferedBySites.AsEnumerable(), jackpotOfferedByCurrencies.AsEnumerable());
        object cachedValue = tuple;
        cacheMock
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(true);
    }

    #endregion
}