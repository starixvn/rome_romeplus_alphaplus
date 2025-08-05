using AlphaPlus.CashService.API.Features.PendingDeposit.Services;
using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using AlphaPlus.CashService.API.Features.Promotion.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Services;

[Trait("Category", "CashService")]
public class PromotionServiceTests
{
    private readonly Mock<IDynamicRepository> mockRepository;
    private readonly Mock<ICommonService> mockCommonService;
    private readonly Mock<ICacheRefService> mockCacheRef;
    private readonly Mock<IPendingDepositService> mockDepositService;
    private readonly Mock<ITransactionService> mockTransactionService;
    private readonly PromotionService service;

    public PromotionServiceTests()
    {
        mockRepository = new Mock<IDynamicRepository>();
        mockCommonService = new Mock<ICommonService>();
        mockCacheRef = new Mock<ICacheRefService>();
        mockDepositService = new Mock<IPendingDepositService>();
        mockTransactionService = new Mock<ITransactionService>();
        service = new PromotionService(
            mockRepository.Object,
            mockCommonService.Object,
            mockCacheRef.Object,
            mockDepositService.Object,
            mockTransactionService.Object);
    }

    [Fact]
    public async Task GetProductAsync_ShouldReturnProductsWithNames()
    {
        // Arrange
        var site = "test-site";
        var currency = 1;
        var settingStatus = 1;
        var language = "en";

        var products = new List<Product>
        {
            new() { ProductId = 1, ProductCategoryId = 1 },
            new() { ProductId = 2, ProductCategoryId = 1 }
        };

        mockRepository
            .Setup(repo => repo.FetchAsync<Product>(It.IsAny<GetProductListCriteria>(), string.Empty, default))
            .ReturnsAsync(products);

        mockCacheRef
            .Setup(cache => cache.GetProductName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns("Test Product");

        // Act
        var result = await service.GetProductAsync(site, currency, settingStatus, language);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetPromotionTransactionAsync_ShouldReturnTransactionsWithTotal()
    {
        // Arrange
        var request = new GetPromotionTransactionRequest
        {
            CustId = 1,
            SiteDbGroupId = 1,
            FromDate = DateTime.Now.AddDays(-7),
            ToDate = DateTime.Now,
            PromotionTypeId = 1,
            StatusId = 1,
            PageIndex = 1,
            PageSize = 10
        };

        var transactions = new List<PromotionTransaction>
        {
            new() { TransId = 1, CustId = 1, Amount = 100m }
        };

        mockRepository
            .Setup(repo => repo.FetchAsync<PromotionTransaction>(It.IsAny<GetPromotionTransactionCriteria>(), string.Empty, default))
            .Callback((ICriteria criteria, string s, CancellationToken ct) =>
            {
                ((GetPromotionTransactionCriteria)criteria).TotalRecords = 1;
            })
            .ReturnsAsync(transactions);

        // Act
        var (result, total) = await service.GetPromotionTransactionAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, total);
    }
}
