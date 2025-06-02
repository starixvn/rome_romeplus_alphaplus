using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossByProduct;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossByProduct;

[Trait("Category", "WinLossByProduct")]
public class WinLossByProductServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IReportDateService> reportDateServiceMock;
    private readonly Mock<ICacheRefService> cacheRefServiceMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly WinLossByProductService service;

    public WinLossByProductServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        reportDateServiceMock = new Mock<IReportDateService>();
        cacheRefServiceMock = new Mock<ICacheRefService>();
        partialUMServiceMock = new Mock<IPartialUMService>();

        service = new WinLossByProductService(
            repositoryMock.Object,
            reportDateServiceMock.Object,
            cacheRefServiceMock.Object,
            partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetWinLossByProduct_Should_Return_Expected_Data()
    {
        // Arrange
        var param = new WinLossByProductParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailXml: "<products></products>",
            CustLevelId: 2,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = DateTime.Now.AddDays(-1),
            MoveBetsDay = DateTime.Now.AddDays(-2)
        };

        var rawData = new List<WinLossByBetTypeModel>
        {
            new() { ProductId = 1, SportType = 1, BetType = 1, IsMMR = false, LiveIndicator = false, Turnover = 100, WinLoss = 50 },
            new() { ProductId = 1, SportType = 1, BetType = 2, IsMMR = true, LiveIndicator = false, Turnover = 200, WinLoss = -30 },
            new() { ProductId = 2, SportType = 2, BetType = 1, IsMMR = false, LiveIndicator = true, Turnover = 300, WinLoss = 20 }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<WinLossByBetTypeModel>(It.IsAny<WinLossByProductCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        cacheRefServiceMock.Setup(s => s.GetOddsTypeName("6")).Returns("MMR");
        cacheRefServiceMock.Setup(s => s.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(s => s.GetProductName(2)).Returns("Product2");
        cacheRefServiceMock.Setup(s => s.GetSportName(1)).Returns("Sport1");
        cacheRefServiceMock.Setup(s => s.GetSportName(2)).Returns("Sport2");
        cacheRefServiceMock.Setup(s => s.GetBetTypeName(1)).Returns("BetType1");
        cacheRefServiceMock.Setup(s => s.GetBetTypeName(2)).Returns("BetType2");

        // Act
        var result = await service.GetWinLossByProduct(param);

        // Assert
        Assert.NotNull(result);
        var (products, grandTotal) = result;

        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(2, products.Count());

        Assert.NotNull(grandTotal);
        Assert.Equal(600, grandTotal.Turnover);
        Assert.Equal(40, grandTotal.WinLoss);

        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName), Times.Once);
        repositoryMock.Verify(
            r => r.FetchAsync<WinLossByBetTypeModel>(
                It.Is<WinLossByProductCriteria>(c =>
                    c.CustId == param.CustId &&
                    c.FromDate == param.FromDate &&
                    c.ToDate == param.ToDate &&
                    c.BalanceUpDay == reportDate.BalanceUpDay &&
                    c.MoveBetsDay == reportDate.MoveBetsDay &&
                    c.ProductIds == param.ProductIds), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetWinLossByProduct_Should_Return_Empty_When_No_Data()
    {
        // Arrange
        var param = new WinLossByProductParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailXml: "<products></products>",
            CustLevelId: 2,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = DateTime.Now.AddDays(-1),
            MoveBetsDay = DateTime.Now.AddDays(-2)
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<WinLossByBetTypeModel>(It.IsAny<WinLossByProductCriteria>(), string.Empty))
            .ReturnsAsync(new List<WinLossByBetTypeModel>());

        // Act
        var result = await service.GetWinLossByProduct(param);

        // Assert
        Assert.NotNull(result);
        var (products, grandTotal) = result;

        Assert.NotNull(products);
        Assert.Empty(products);

        Assert.NotNull(grandTotal);
        Assert.Equal(0, grandTotal.Turnover);
        Assert.Equal(0, grandTotal.WinLoss);
    }

    [Fact]
    public async Task GetWinLossByProduct_Should_Format_NumberGame_BetTypeNames()
    {
        // Arrange
        var param = new WinLossByProductParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1",
            ProductDetailXml: "<products></products>",
            CustLevelId: 2,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay();

        var rawData = new List<WinLossByBetTypeModel>
        {
            new() { ProductId = ProductIdConstants.NumberGame, SportType = 1, BetType = BetTypeConstants.N_Over_Under, IsMMR = false, LiveIndicator = false },
            new() { ProductId = ProductIdConstants.NumberGame, SportType = 1, BetType = BetTypeConstants.N_Odd_Even, IsMMR = false, LiveIndicator = false },
            new() { ProductId = ProductIdConstants.NumberGame, SportType = 1, BetType = BetTypeConstants.N_Odd_Even, IsMMR = false, LiveIndicator = true },
            new() { ProductId = ProductIdConstants.NumberGame, SportType = 1, BetType = BetTypeConstants.NumberWheel, IsMMR = false, LiveIndicator = false },
            new() { ProductId = ProductIdConstants.NumberGame, SportType = 1, BetType = BetTypeConstants.NumberWheel, IsMMR = false, LiveIndicator = true }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<WinLossByBetTypeModel>(It.IsAny<WinLossByProductCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        cacheRefServiceMock.Setup(s => s.GetOddsTypeName(It.IsAny<string>())).Returns("MMR");
        cacheRefServiceMock.Setup(s => s.GetProductName(It.IsAny<int>())).Returns("Product");
        cacheRefServiceMock.Setup(s => s.GetSportName(It.IsAny<int>())).Returns("Sport");
        cacheRefServiceMock.Setup(s => s.GetBetTypeName(BetTypeConstants.N_Over_Under)).Returns("Over/Under");
        cacheRefServiceMock.Setup(s => s.GetBetTypeName(BetTypeConstants.N_Odd_Even)).Returns("Odd/Even");
        cacheRefServiceMock.Setup(s => s.GetBetTypeName(BetTypeConstants.NumberWheel)).Returns("Number Wheel");

        // Act
        var result = await service.GetWinLossByProduct(param);

        // Assert
        Assert.NotNull(result);
        var (products, _) = result;

        var product = products.First();
        var sport = product.WinLossBySports.First();
        var betTypes = sport.WinLossByBetTypes.ToList();

        Assert.Contains(betTypes, b => b.BetTypeName.Contains("Next Over/Under"));
        Assert.Contains(betTypes, b => b.BetTypeName.Contains("FT. Odd/Even"));
        Assert.Contains(betTypes, b => b.BetTypeName.Contains("Next Odd/Even"));
        Assert.Contains(betTypes, b => b.BetTypeName.Contains("Number Wheel Pre Game"));
        Assert.Contains(betTypes, b => b.BetTypeName.Contains("Number Wheel Live Game"));
    }

    [Fact]
    public async Task GetWinLossByProduct_Should_Format_MMR_BetTypeNames()
    {
        // Arrange
        var param = new WinLossByProductParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1",
            ProductDetailXml: "<products></products>",
            CustLevelId: 2,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay();

        var rawData = new List<WinLossByBetTypeModel>
        {
            new() { ProductId = 1, SportType = 1, BetType = 1, IsMMR = true, LiveIndicator = false }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<WinLossByBetTypeModel>(It.IsAny<WinLossByProductCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        cacheRefServiceMock.Setup(s => s.GetOddsTypeName("6")).Returns("MMR");
        cacheRefServiceMock.Setup(s => s.GetProductName(1)).Returns("Product1");
        cacheRefServiceMock.Setup(s => s.GetSportName(1)).Returns("Sport1");
        cacheRefServiceMock.Setup(s => s.GetBetTypeName(1)).Returns("BetType1");

        // Act
        var result = await service.GetWinLossByProduct(param);

        // Assert
        var (products, _) = result;
        var betType = products.First().WinLossBySports.First().WinLossByBetTypes.First();

        Assert.Equal("BetType1 (MMR)", betType.BetTypeName);
    }
}