using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.SuperWinLossDetailByPT;

[Trait("Category", "SuperWinLossDetailByPT")]
public class SuperWinLossDetailByPTServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IReportDateService> reportDateServiceMock;
    private readonly Mock<ICacheRefService> cacheRefServiceMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly SuperWinLossDetailByPTService service;

    public SuperWinLossDetailByPTServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        reportDateServiceMock = new Mock<IReportDateService>();
        cacheRefServiceMock = new Mock<ICacheRefService>();
        partialUMServiceMock = new Mock<IPartialUMService>();

        service = new SuperWinLossDetailByPTService(
            repositoryMock.Object,
            reportDateServiceMock.Object,
            cacheRefServiceMock.Object,
            partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetSuperWinLossDetailByPT_Should_Return_Expected_Data()
    {
        // Arrange
        var param = new SuperWinLossDetailByPTParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            SettingStatus: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = DateTime.Now.AddDays(-1),
            MoveBetsDay = DateTime.Now.AddDays(-2)
        };

        var rawData = new List<SuperWinLossDetailByPTEntity>
        {
            new() { ProductId = 1, Turnover = 100, GrossComm = 10 },
            new() { ProductId = 2, Turnover = 200, GrossComm = 20 }
        };

        var ptList = new List<PositionTakingModel>
        {
            new() { ProductId = 1, ValuePT = 0.3m },
            new() { ProductId = 2, ValuePT = 0.5m }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<SuperWinLossDetailByPTEntity>(It.IsAny<SuperWinLossDetailByPTCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        repositoryMock
            .Setup(r => r.FetchAsync<PositionTakingModel>(It.IsAny<SuperPTCriteria>(), string.Empty))
            .ReturnsAsync(ptList);

        cacheRefServiceMock
            .Setup(s => s.GetProductName(It.IsAny<int>()))
            .Returns("Product");

        // Act
        var result = await service.GetSuperWinLossDetailByPT(param);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.NotEmpty(result.Item1);
        Assert.NotNull(result.Item2);
        Assert.NotNull(result.Item3);
        Assert.Equal(ptList, result.Item3);

        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName), Times.Once);
        cacheRefServiceMock.Verify(s => s.GetProductName(It.IsAny<int>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetSuperWinLossDetailByPT_Should_Return_Empty_Data_When_No_RawData()
    {
        // Arrange
        var param = new SuperWinLossDetailByPTParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            SettingStatus: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = DateTime.Now.AddDays(-1),
            MoveBetsDay = DateTime.Now.AddDays(-2)
        };

        var emptyRawData = new List<SuperWinLossDetailByPTEntity>();

        var ptList = new List<PositionTakingModel>
        {
            new() { ProductId = 1, ValuePT = 0.3m },
            new() { ProductId = 2, ValuePT = 0.5m }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<SuperWinLossDetailByPTEntity>(It.IsAny<SuperWinLossDetailByPTCriteria>(), string.Empty))
            .ReturnsAsync(emptyRawData);

        repositoryMock
            .Setup(r => r.FetchAsync<PositionTakingModel>(It.IsAny<SuperPTCriteria>(), string.Empty))
            .ReturnsAsync(ptList);

        // Act
        var result = await service.GetSuperWinLossDetailByPT(param);

        // Assert
        Assert.Empty(result.Item1);
        Assert.NotNull(result.Item2);
        Assert.NotNull(result.Item3);
        Assert.Equal(ptList, result.Item3);

        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName), Times.Once);
        cacheRefServiceMock.Verify(s => s.GetProductName(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetSuperWinLossDetailByPT_Should_Pass_Correct_Parameters_To_Repository()
    {
        // Arrange
        var param = new SuperWinLossDetailByPTParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            SettingStatus: 0,
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
            .Setup(r => r.FetchAsync<SuperWinLossDetailByPTEntity>(It.IsAny<SuperWinLossDetailByPTCriteria>(), string.Empty))
            .ReturnsAsync(new List<SuperWinLossDetailByPTEntity>());

        repositoryMock
            .Setup(r => r.FetchAsync<PositionTakingModel>(It.IsAny<SuperPTCriteria>(), string.Empty))
            .ReturnsAsync(new List<PositionTakingModel>());

        // Act
        await service.GetSuperWinLossDetailByPT(param);

        // Assert
        repositoryMock.Verify(
            r => r.FetchAsync<SuperWinLossDetailByPTEntity>(
                It.Is<SuperWinLossDetailByPTCriteria>(c =>
                    c.CustId == param.CustId &&
                    c.FromDate == param.FromDate &&
                    c.ToDate == param.ToDate &&
                    c.BalanceUpDay == reportDate.BalanceUpDay &&
                    c.MoveBetsDay == reportDate.MoveBetsDay &&
                    c.ProductIds == param.ProductIds &&
                    c.ProductDetails == param.ProductDetailsXml), string.Empty),
            Times.Once);

        repositoryMock.Verify(
            r => r.FetchAsync<PositionTakingModel>(
                It.Is<SuperPTCriteria>(c =>
                    c.Supers == param.CustId.ToString() &&
                    c.SettingStatus == param.SettingStatus), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetSuperWinLossDetailByPT_Should_Group_And_Sort_Data_Correctly()
    {
        // Arrange
        var param = new SuperWinLossDetailByPTParam(
            CustId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            SettingStatus: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay();

        var rawData = new List<SuperWinLossDetailByPTEntity>
        {
            new() { ProductId = 1, Turnover = 100, NetTurnover = 10 },
            new() { ProductId = 2, Turnover = 300, NetTurnover = 30 },
            new() { ProductId = 3, Turnover = 200, NetTurnover = 20 }
        };

        var ptList = new List<PositionTakingModel>
        {
            new() { ProductId = 1, ValuePT = 0.3m },
            new() { ProductId = 2, ValuePT = 0.3m },
            new() { ProductId = 3, ValuePT = 0.5m }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<SuperWinLossDetailByPTEntity>(It.IsAny<SuperWinLossDetailByPTCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        repositoryMock
            .Setup(r => r.FetchAsync<PositionTakingModel>(It.IsAny<SuperPTCriteria>(), string.Empty))
            .ReturnsAsync(ptList);

        cacheRefServiceMock
            .Setup(s => s.GetProductName(It.IsAny<int>()))
            .Returns("Product");

        // Act
        var result = await service.GetSuperWinLossDetailByPT(param);

        // Assert
        var reportData = result.Item1.ToList();

        // Should have 2 groups (0.3 and 0.5 PT values)
        Assert.Equal(2, reportData.Count);

        // First group should have PT value of 0.3 with 2 products
        Assert.Equal(0.3m, reportData[0].SuperPT);
        Assert.Equal(2, reportData[0].SuperWinLossDetails.Count());

        // Within first group, products should be sorted by Turnover descending
        Assert.Equal(2, reportData[0].SuperWinLossDetails.First().ProductId);
        Assert.Equal(1, reportData[0].SuperWinLossDetails.Skip(1).First().ProductId);

        // Second group should have PT value of 0.5 with 1 product
        Assert.Equal(0.5m, reportData[1].SuperPT);
        Assert.Single(reportData[1].SuperWinLossDetails);
        Assert.Equal(3, reportData[1].SuperWinLossDetails.First().ProductId);
    }
}
