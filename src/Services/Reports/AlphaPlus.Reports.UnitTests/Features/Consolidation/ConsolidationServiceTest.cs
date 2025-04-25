using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Reports.UnitTests.Features.Consolidation;

[Trait("Category", "Consolidation")]
public class ConsolidationServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock = new();
    private readonly Mock<IReportDateService> reportDateServiceMock = new();
    private readonly Mock<IMemoryCache> cacheServiceMock = new();
    private readonly Mock<ICacheRefService> cacheRefServiceMock = new();
    private readonly Mock<IPartialUMService> partialUMServiceMock = new();
    private readonly ConsolidationService service;

    private readonly ConsolidationParam testParam = new(
        CustId: 123,
        Currency: 20,
        FromDate: new DateTime(2023, 1, 1),
        ToDate: new DateTime(2023, 3, 31),
        ProductIds: "1,2,3",
        ProductDetailsXml: "<Products><Product Id=\"1\" /></Products>",
        ReportType: ConsolidationReportType.ByUsername,
        SettingStatus: 1,
        SiteName: "TestSite",
        Currencies: [
            new ConsolidationCurrency { Currency = 20, CurrencyName = "USD" },
            new ConsolidationCurrency { Currency = 2, CurrencyName = "EUR" }
        ]);

    private readonly List<ConsolidationCurrency> testCurrencies = new()
    {
        new ConsolidationCurrency { Currency = 20, CurrencyName = "USD" },
        new ConsolidationCurrency { Currency = 2, CurrencyName = "EUR" }
    };

    private readonly BalanceUpDayAndMoveBetsDay testReportDate = new()
    {
        BalanceUpDay = new DateTime(2023, 3, 31),
        MoveBetsDay = new DateTime(2023, 3, 31)
    };

    public ConsolidationServiceTest()
    {
        service = new ConsolidationService(
            repositoryMock.Object,
            reportDateServiceMock.Object,
            cacheServiceMock.Object,
            cacheRefServiceMock.Object,
            partialUMServiceMock.Object,
            new AllowedSitesToShareLoad(new Dictionary<string, string>()));
    }

    [Fact]
    public async Task GetCurrencies_Should_Call_Repository_With_Correct_Parameters()
    {
        // Arrange
        int custId = 123;
        string siteName = "Test";

        repositoryMock
            .Setup(r => r.FetchAsync<ConsolidationCurrency>(It.IsAny<GetConsolidationCurrencyCriteria>(), It.IsAny<string>()))
            .ReturnsAsync(testCurrencies);

        // Act
        var result = await service.GetCurrencies(custId, siteName);

        // Assert
        repositoryMock.Verify(
            r => r.FetchAsync<ConsolidationCurrency>(
                It.Is<GetConsolidationCurrencyCriteria>(c => c.CustId == custId),
                It.IsAny<string>()),
            Times.Once);

        Assert.Equal(testCurrencies, result);
    }

    [Fact]
    public async Task GetProducts_Should_Return_Cached_Products()
    {
        // Arrange
        int custId = 123;
        DateTime minDate = new(2023, 1, 1);
        byte settingStatus = 1;
        string siteName = "Test";

        var products = new List<ConsolidationProduct>
        {
            new() { ProductId = 1, Type = 1, IsShow = true },
            new() { ProductId = ProductIdConstants.Cashout, Type = 1, IsShow = true }, // Should be filtered out
            new() { ProductId = 3, Type = 2, IsShow = true },
            new() { ProductId = 4, Type = 2, IsShow = false }, // Should be filtered out
            new() { ProductId = 5, Type = 3, IsShow = true, EndOfSupportDate = new DateTime(2022, 12, 31) } // Should be filtered out
        };

        repositoryMock
            .Setup(r => r.FetchAsync<ConsolidationProduct>(It.IsAny<GetConsolidationProductsCriteria>(), It.IsAny<string>()))
            .ReturnsAsync(products);

        cacheRefServiceMock
            .Setup(c => c.GetProductName("1"))
            .Returns("Product1");
        cacheRefServiceMock
            .Setup(c => c.GetProductName("3"))
            .Returns("Product3");
        cacheRefServiceMock
            .Setup(c => c.GetProductName(1))
            .Returns("Type1");
        cacheRefServiceMock
            .Setup(c => c.GetProductName(2))
            .Returns("Type2");

        var memoryCacheEntry = new Mock<ICacheEntry>();
        cacheServiceMock
            .Setup(c => c.CreateEntry(It.IsAny<object>()))
            .Returns(memoryCacheEntry.Object);

        // Setup cache behavior
        object cachedValue = null;
        cacheServiceMock
            .Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(false);

        // Act
        var result = (await service.GetProducts(custId, minDate, settingStatus, siteName)).ToList();

        // Assert
        repositoryMock.Verify(
            r => r.FetchAsync<ConsolidationProduct>(
                It.Is<GetConsolidationProductsCriteria>(c => c.CustId == custId && c.SettingStatus == settingStatus),
                It.IsAny<string>()),
            Times.Once);

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Id);
        //Assert.Equal("Product1", result[0].Name);
        Assert.Equal(1, result[0].Type);
        Assert.Equal("Type1", result[0].TypeName);

        Assert.Equal(3, result[1].Id);
        //Assert.Equal("Product3", result[1].Name);
        Assert.Equal(2, result[1].Type);
        Assert.Equal("Type2", result[1].TypeName);
    }

    [Fact]
    public async Task GetConsolidationByUsername_Should_Call_Repository_With_Correct_Parameters()
    {
        // Arrange
        var entityList = new List<ConsolidationEntity>
        {
            new() { CustId = 456, CustLevel = 2, Username = "User1", BreakDown = "1", CurrencyId = 20, Turnover = 1000, MemberWinLoss = 100 },
            new() { CustId = 456, CustLevel = 2, Username = "User1", BreakDown = "2", CurrencyId = 20, Turnover = 2000, MemberWinLoss = 200 }
        };

        var ptList = new List<PositionTakingModel>
        {
            new() { CustId = 456, ProductId = 1, ValuePT = 10 },
            new() { CustId = 456, ProductId = 2, ValuePT = 20 }
        };

        SetupMocks(entityList, ptList);
        cacheRefServiceMock.Setup(c => c.GetProductName("1")).Returns("Product1");
        cacheRefServiceMock.Setup(c => c.GetProductName("2")).Returns("Product2");

        // Act
        var result = (await service.GetConsolidationByUsername(testParam)).ToList();

        // Assert
        partialUMServiceMock.Verify(
            s => s.ThrowIfUM(It.IsAny<string>(), testParam.SiteName),
            Times.Once);

        repositoryMock.Verify(
            r => r.FetchAsync<ConsolidationEntity>(
                It.Is<GetConsolidationCriteria>(c =>
                    c.CustId == testParam.CustId &&
                    c.Currency == testParam.Currency &&
                    c.FromDate == testParam.FromDate &&
                    c.ToDate == testParam.ToDate &&
                    c.ProductIds == testParam.ProductIds &&
                    c.ProductDetails == testParam.ProductDetailsXml &&
                    c.BalanceUpDay == testReportDate.BalanceUpDay &&
                    c.MoveBetsDay == testReportDate.MoveBetsDay),
                It.IsAny<string>()),
            Times.Once);

        Assert.Single(result);
        Assert.Equal("User1", result[0].ConsolidationByPTs.First().WinLoss.First().Username);
        Assert.Equal(3000, result[0].Total.Turnover);
        Assert.Equal(300, result[0].Total.MemberWinLoss);
        Assert.Equal(2, result[0].ConsolidationByPTs.Count());
    }

    [Fact]
    public async Task GetConsolidationByDateOrProduct_Should_Return_Grouped_Data()
    {
        // Arrange
        var param = testParam with { ReportType = ConsolidationReportType.ByDate };
        var today = DateTime.Now.Date.ToString("yyyy-MM-dd");

        var entityList = new List<ConsolidationEntity>
        {
            new() { CustId = 456, BreakDown = today, CurrencyId = 20, Turnover = 1000, MemberWinLoss = 100 },
            new() { CustId = 789, BreakDown = today, CurrencyId = 20, Turnover = 2000, MemberWinLoss = 200 }
        };

        SetupMocks(entityList);

        // Act
        var result = (await service.GetConsolidationByDateOrProduct(param)).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(DateTime.Now.Date.ToString("MM/dd/yyyy"), result[0].BreakDown);
        Assert.Equal(2, result[0].WinLoss.Count());
        Assert.Equal(3000, result[0].WinLoss.Sum(i => i.Turnover));
        Assert.Equal(300, result[0].WinLoss.Sum(i => i.MemberWinLoss));
    }

    [Fact]
    public async Task GetConsolidationByDateOrProduct_Should_Group_By_Product_When_ReportType_Is_ByProduct()
    {
        // Arrange
        var param = testParam with { ReportType = ConsolidationReportType.ByProduct };

        var entityList = new List<ConsolidationEntity>
        {
            new() { CustId = 456, BreakDown = "1", CurrencyId = 20, Turnover = 1000, MemberWinLoss = 100 },
            new() { CustId = 789, BreakDown = "1", CurrencyId = 20, Turnover = 2000, MemberWinLoss = 200 },
            new() { CustId = 456, BreakDown = "2", CurrencyId = 20, Turnover = 3000, MemberWinLoss = 300 }
        };

        SetupMocks(entityList);
        cacheRefServiceMock.Setup(c => c.GetProductName("1")).Returns("Product1");
        cacheRefServiceMock.Setup(c => c.GetProductName("2")).Returns("Product2");

        // Act
        var result = (await service.GetConsolidationByDateOrProduct(param)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Product1", result[0].BreakDown);
        Assert.Equal(2, result[0].WinLoss.Count());
        Assert.Equal(3000, result[0].WinLoss.Sum(i => i.Turnover));
        Assert.Equal(300, result[0].WinLoss.Sum(i => i.MemberWinLoss));

        Assert.Equal("Product2", result[1].BreakDown);
        Assert.Single(result[1].WinLoss);
        Assert.Equal(3000, result[1].WinLoss.Sum(i => i.Turnover));
        Assert.Equal(300, result[1].WinLoss.Sum(i => i.MemberWinLoss));
    }

    [Fact]
    public async Task GetConsolidationByDateOrProduct_Should_Return_Empty_List_When_No_Data()
    {
        // Arrange
        var entityList = new List<ConsolidationEntity>();
        SetupMocks(entityList);

        // Act
        var result = await service.GetConsolidationByDateOrProduct(testParam);

        // Assert
        Assert.Empty(result);
    }

    private void SetupMocks(List<ConsolidationEntity> entityList, List<PositionTakingModel> ptList = null)
    {
        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(testReportDate);

        partialUMServiceMock
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        repositoryMock
            .Setup(r => r.FetchAsync<ConsolidationEntity>(It.IsAny<GetConsolidationCriteria>(), It.IsAny<string>()))
            .ReturnsAsync(entityList);

        if (ptList != null)
        {
            repositoryMock
                .Setup(r => r.FetchAsync<PositionTakingModel>(It.IsAny<GetSMAPositionTakingCriteria>(), It.IsAny<string>()))
                .ReturnsAsync(ptList);
        }
    }
}