using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.UnitTests.Features.EnteredResult;

[Trait("Category", "EnteredResult")]
public class EnteredResultServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IMemoryCache> cacheMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly Mock<ICacheRefService> cacheRefServiceMock;
    private readonly Mock<IOptions<AppSettings>> optionsMock;
    private readonly Mock<ICacheEntry> cacheEntryMock;
    private readonly EnteredResultService service;
    private readonly string siteName = "TestSite";

    public EnteredResultServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        cacheMock = new Mock<IMemoryCache>();
        partialUMServiceMock = new Mock<IPartialUMService>();
        cacheRefServiceMock = new Mock<ICacheRefService>();
        optionsMock = new Mock<IOptions<AppSettings>>();
        cacheEntryMock = new Mock<ICacheEntry>();

        optionsMock.Setup(o => o.Value).Returns(new AppSettings { LocalTest = false });

        cacheMock
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntryMock.Object);

        service = new EnteredResultService(
            repositoryMock.Object,
            cacheMock.Object,
            partialUMServiceMock.Object,
            cacheRefServiceMock.Object,
            optionsMock.Object);
    }

    [Fact]
    public async Task GetEnteredResults_ShouldReturnFromCache_WhenCachedValueExists()
    {
        // Arrange
        var expectedResult = new List<EnteredResultSport>
        {
            new EnteredResultSport(
                1,
                "Soccer",
                new EnteredResultEntity(SystemTime.Today, 10, 5),
                new EnteredResultEntity(SystemTime.Yesterday, 8, 3))
        };

        object cachedValue = expectedResult;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(true);

        // Act
        var result = await service.GetEnteredResults(siteName);

        // Assert
        Assert.Equal(expectedResult, result);
        repositoryMock.Verify(
            x => x.FetchAsync<EnteredResultEntity>(It.IsAny<EnteredResultCriteria>(), string.Empty),
            Times.Never);
    }

    [Fact]
    public async Task GetEnteredResults_ShouldFetchFromRepository_WhenCachedValueDoesNotExist()
    {
        // Arrange
        var today = SystemTime.Today;
        var yesterday = SystemTime.Yesterday;

        var enteredResults = new List<EnteredResultEntity>
        {
            new EnteredResultEntity(today, 10, 5) { SportType = 1, IsToday = true },
            new EnteredResultEntity(yesterday, 8, 3) { SportType = 1, IsToday = false }
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.FetchAsync<EnteredResultEntity>(It.IsAny<EnteredResultCriteria>(), string.Empty))
            .ReturnsAsync(enteredResults);

        cacheRefServiceMock
            .Setup(x => x.GetSportName(1))
            .Returns("Soccer");

        // Act
        var result = await service.GetEnteredResults(siteName);

        // Assert
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal(1, resultList[0].SportTypeId);
        Assert.Equal("Soccer", resultList[0].SportName);
        Assert.Equal(10, resultList[0].EnteredResultToday.Entered);
        Assert.Equal(5, resultList[0].EnteredResultToday.NonEntered);
        Assert.Equal(8, resultList[0].EnteredResultYesterday.Entered);
        Assert.Equal(3, resultList[0].EnteredResultYesterday.NonEntered);

        partialUMServiceMock.Verify(
            x => x.ThrowIfUM(It.IsAny<string>(), siteName),
            Times.Once);
    }

    [Fact]
    public async Task GetEnteredResultsFromDB_ShouldReturnEmptyList_WhenNoDataFound()
    {
        // Arrange
        var emptyList = new List<EnteredResultEntity>();

        repositoryMock
            .Setup(x => x.FetchAsync<EnteredResultEntity>(It.IsAny<EnteredResultCriteria>(), string.Empty))
            .ReturnsAsync(emptyList);

        // Act
        var result = await service.GetEnteredResultsFromDB(siteName);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTotalEnteredResults_WithSiteName_ShouldReturnAggregateTotals()
    {
        // Arrange
        var today = SystemTime.Today;
        var yesterday = SystemTime.Yesterday;

        var enteredResults = new List<EnteredResultSport>
        {
            new EnteredResultSport(
                1,
                "Soccer",
                new EnteredResultEntity(today, 10, 5),
                new EnteredResultEntity(yesterday, 8, 3)),
            new EnteredResultSport(
                2,
                "Basketball",
                new EnteredResultEntity(today, 15, 2),
                new EnteredResultEntity(yesterday, 12, 4))
        };

        object cachedValue = null;
        cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        repositoryMock
            .Setup(x => x.FetchAsync<EnteredResultEntity>(It.IsAny<EnteredResultCriteria>(), string.Empty))
            .ReturnsAsync(new List<EnteredResultEntity>
            {
                new EnteredResultEntity(today, 10, 5) { SportType = 1, IsToday = true },
                new EnteredResultEntity(yesterday, 8, 3) { SportType = 1, IsToday = false },
                new EnteredResultEntity(today, 15, 2) { SportType = 2, IsToday = true },
                new EnteredResultEntity(yesterday, 12, 4) { SportType = 2, IsToday = false }
            });

        cacheRefServiceMock.Setup(x => x.GetSportName(1)).Returns("Soccer");
        cacheRefServiceMock.Setup(x => x.GetSportName(2)).Returns("Basketball");

        // Act
        var result = await service.GetTotalEnteredResults(siteName);

        // Assert
        Assert.Equal(0, result.SportTypeId);
        Assert.Null(result.SportName);
        Assert.Equal(25, result.EnteredResultToday.Entered); // 10 + 15
        Assert.Equal(7, result.EnteredResultToday.NonEntered); // 5 + 2
        Assert.Equal(20, result.EnteredResultYesterday.Entered); // 8 + 12
        Assert.Equal(7, result.EnteredResultYesterday.NonEntered); // 3 + 4
    }

    [Fact]
    public void GetTotalEnteredResults_WithEnteredResultsList_ShouldAggregateResults()
    {
        // Arrange
        var today = SystemTime.Today;
        var yesterday = SystemTime.Yesterday;

        var enteredResults = new List<EnteredResultSport>
        {
            new EnteredResultSport(
                1,
                "Soccer",
                new EnteredResultEntity(today, 10, 5),
                new EnteredResultEntity(yesterday, 8, 3)),
            new EnteredResultSport(
                2,
                "Basketball",
                new EnteredResultEntity(today, 15, 2),
                new EnteredResultEntity(yesterday, 12, 4))
        };

        // Act
        var result = service.GetTotalEnteredResults(enteredResults);

        // Assert
        Assert.Equal(0, result.SportTypeId);
        Assert.Null(result.SportName);
        Assert.Equal(25, result.EnteredResultToday.Entered); // 10 + 15
        Assert.Equal(7, result.EnteredResultToday.NonEntered); // 5 + 2
        Assert.Equal(20, result.EnteredResultYesterday.Entered); // 8 + 12
        Assert.Equal(7, result.EnteredResultYesterday.NonEntered); // 3 + 4
    }

    [Fact]
    public async Task GetEnteredResultsFromDB_ShouldCheckPartialUM()
    {
        // Arrange
        var enteredResults = new List<EnteredResultEntity>
        {
            new EnteredResultEntity(SystemTime.Today, 10, 5) { SportType = 1, IsToday = true }
        };

        repositoryMock
            .Setup(x => x.FetchAsync<EnteredResultEntity>(It.IsAny<EnteredResultCriteria>(), string.Empty))
            .ReturnsAsync(enteredResults);

        // Act
        await service.GetEnteredResultsFromDB(siteName);

        // Assert
        partialUMServiceMock.Verify(
            x => x.ThrowIfUM(It.IsAny<string>(), siteName),
            Times.Once);
    }
}