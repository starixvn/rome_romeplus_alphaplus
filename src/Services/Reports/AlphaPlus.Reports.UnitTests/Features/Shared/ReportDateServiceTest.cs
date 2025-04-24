using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Criteria;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Reports.UnitTests.Features.Shared;

[Trait("Category", "Shared")]
public class ReportDateServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IMemoryCache> cacheMock;
    private readonly ReportDateService service;
    private readonly Mock<ICacheEntry> cacheEntryMock;

    public ReportDateServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        cacheMock = new Mock<IMemoryCache>();
        cacheEntryMock = new Mock<ICacheEntry>();

        cacheMock
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntryMock.Object);

        service = new ReportDateService(repositoryMock.Object, cacheMock.Object);
    }

    [Fact]
    public async Task GetBalanceUpDayAndMoveBetsDay_ShouldReturnFromCache_WhenCachedValueExists()
    {
        // Arrange
        var expectedResult = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = expectedResult;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(true);

        // Act
        var result = await service.GetBalanceUpDayAndMoveBetsDay();

        // Assert
        Assert.Equal(expectedResult.BalanceUpDay, result.BalanceUpDay);
        Assert.Equal(expectedResult.MoveBetsDay, result.MoveBetsDay);
        repositoryMock.Verify(
            x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty),
            Times.Never);
    }

    [Fact]
    public async Task GetBalanceUpDayAndMoveBetsDay_ShouldFetchFromRepository_WhenCachedValueDoesNotExist()
    {
        // Arrange
        var expectedResult = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await service.GetBalanceUpDayAndMoveBetsDay();

        // Assert
        Assert.Equal(expectedResult.BalanceUpDay, result.BalanceUpDay);
        Assert.Equal(expectedResult.MoveBetsDay, result.MoveBetsDay);
        repositoryMock.Verify(
            x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetBalanceUpDay_ShouldReturnCorrectValue()
    {
        // Arrange
        var balanceUpDayAndMoveBetsDay = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(balanceUpDayAndMoveBetsDay);

        // Act
        var result = await service.GetBalanceUpDay();

        // Assert
        Assert.Equal(balanceUpDayAndMoveBetsDay.BalanceUpDay, result);
    }

    [Fact]
    public async Task GetMoveBetsDay_ShouldReturnCorrectValue()
    {
        // Arrange
        var balanceUpDayAndMoveBetsDay = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(balanceUpDayAndMoveBetsDay);

        // Act
        var result = await service.GetMoveBetsDay();

        // Assert
        Assert.Equal(balanceUpDayAndMoveBetsDay.MoveBetsDay, result);
    }

    [Fact]
    public async Task GetTimeRangesToGetReportDataBetweenTables_ShouldReturnSingleRange_WhenDateRangeBeforeBalanceUpDay()
    {
        // Arrange
        var fromDate = new DateTime(2022, 12, 1);
        var toDate = new DateTime(2022, 12, 31);

        var balanceUpDayAndMoveBetsDay = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(balanceUpDayAndMoveBetsDay);

        // Act
        var result = await service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

        // Assert
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal(fromDate, resultList[0].FromDate);
        Assert.Equal(toDate, resultList[0].ToDate);
    }

    [Fact]
    public async Task GetTimeRangesToGetReportDataBetweenTables_ShouldReturnMultipleRanges_WhenDateRangeCrossesBalanceUpDay()
    {
        // Arrange
        var fromDate = new DateTime(2022, 12, 15);
        var toDate = new DateTime(2023, 1, 10);

        var balanceUpDayAndMoveBetsDay = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(balanceUpDayAndMoveBetsDay);

        // Act
        var result = await service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);

        Assert.Equal(fromDate, resultList[0].FromDate);
        Assert.Equal(balanceUpDayAndMoveBetsDay.BalanceUpDay.AddDays(-1), resultList[0].ToDate);

        Assert.Equal(balanceUpDayAndMoveBetsDay.BalanceUpDay, resultList[1].FromDate);
        Assert.Equal(toDate, resultList[1].ToDate);
    }

    [Fact]
    public async Task GetStoreMappingSuffix_ShouldReturnEmptyString_WhenDateAfterMoveBetsDay()
    {
        // Arrange
        var date = new DateTime(2023, 1, 20);

        var balanceUpDayAndMoveBetsDay = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(balanceUpDayAndMoveBetsDay);

        // Act
        var result = await service.GetStoreMappingSuffix(date);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task GetStoreMappingSuffix_ShouldReturnBetTrans14Suffix_WhenDateBetweenBalanceUpDayAndMoveBetsDay()
    {
        // Arrange
        var date = new DateTime(2023, 1, 10);

        var balanceUpDayAndMoveBetsDay = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(balanceUpDayAndMoveBetsDay);

        // Act
        var result = await service.GetStoreMappingSuffix(date);

        // Assert
        Assert.Equal(ReportDateService.BetTrans14SPSuffix, result);
    }

    [Fact]
    public async Task GetStoreMappingSuffix_ShouldReturnWarehouseSuffix_WhenDateBeforeBalanceUpDay()
    {
        // Arrange
        var date = new DateTime(2022, 12, 15);

        var balanceUpDayAndMoveBetsDay = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 1),
            MoveBetsDay = new DateTime(2023, 1, 15)
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.GetAsync<BalanceUpDayAndMoveBetsDay>(It.IsAny<GetBalanceUpDayAndMoveBetsDayCriteria>(), string.Empty))
            .ReturnsAsync(balanceUpDayAndMoveBetsDay);

        // Act
        var result = await service.GetStoreMappingSuffix(date);

        // Assert
        Assert.Equal(ReportDateService.WarehouseSPSuffix, result);
    }
}