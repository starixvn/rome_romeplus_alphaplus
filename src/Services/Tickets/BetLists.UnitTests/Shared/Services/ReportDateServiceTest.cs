using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.BetLists.UnitTests.Shared.Services;

[Trait("Category", "BetLists")]
public class ReportDateServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IMemoryCache> cacheMock;
    private readonly Mock<ICacheEntry> cacheEntryMock;
    private readonly ReportDateService service;

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
            BalanceUpDay = new DateTime(2025, 1, 1),
            MoveBetsDay = new DateTime(2025, 1, 15)
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
            BalanceUpDay = new DateTime(2025, 1, 1),
            MoveBetsDay = new DateTime(2025, 1, 15)
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
            BalanceUpDay = new DateTime(2025, 1, 1),
            MoveBetsDay = new DateTime(2025, 1, 15)
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
            BalanceUpDay = new DateTime(2025, 1, 1),
            MoveBetsDay = new DateTime(2025, 1, 15)
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
    public void StaticInstanceProperty_ShouldBeSettable()
    {
        // Arrange
        var mockService = new Mock<IReportDateService>();

        // Act
        ReportDateService.Instance = mockService.Object;

        // Assert
        Assert.Equal(mockService.Object, ReportDateService.Instance);
    }
}