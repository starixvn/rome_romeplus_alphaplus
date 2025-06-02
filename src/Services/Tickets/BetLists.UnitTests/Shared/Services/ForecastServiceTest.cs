using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.BetLists.UnitTests.Shared.Services;

[Trait("Category", "BetLists")]
public class ForecastServiceTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<ICacheEntry> _cacheEntryMock;
    private readonly ForecastService _service;

    public ForecastServiceTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _cacheMock = new Mock<IMemoryCache>();
        _cacheEntryMock = new Mock<ICacheEntry>();

        _cacheMock
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(_cacheEntryMock.Object);

        _service = new ForecastService(_repositoryMock.Object, _cacheMock.Object);
    }

    [Fact]
    public async Task GetBetTypeIdsSupportScoreMap_ShouldReturnCorrectHashSet()
    {
        // Arrange
        var scoreMaps = new List<ScoreMap>
         {
             new ScoreMap { FirstId = "1,2,3" },
             new ScoreMap { FirstId = "4,5" }
         };

        _repositoryMock
            .Setup(x => x.FetchAsync<ScoreMap>(It.IsAny<GetScoreMapBetTypesCriteria>(), string.Empty))
            .ReturnsAsync(scoreMaps);

        // Act
        var result = await _service.GetBetTypeIdsSupportScoreMap();

        // Assert
        Assert.Equal(5, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public async Task GetBetTypeIdsSupportScoreMap_ShouldUseCaching()
    {
        // Arrange
        var scoreMaps = new List<ScoreMap>
         {
             new ScoreMap { FirstId = "1,2,3" }
         };

        _repositoryMock
            .Setup(x => x.FetchAsync<ScoreMap>(It.IsAny<GetScoreMapBetTypesCriteria>(), string.Empty))
            .ReturnsAsync(scoreMaps);

        var expectedResult = new HashSet<int> { 1, 2, 3 };

        // Setup cache to return the cached result on second call
        object cachedValue = expectedResult;
        _cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(false); // First call returns false to trigger the DB call

        // Act
        var result1 = await _service.GetBetTypeIdsSupportScoreMap();

        // Change the mock to return true and the cached value for second call
        _cacheMock
            .Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(true);

        var result2 = await _service.GetBetTypeIdsSupportScoreMap();

        // Assert
        _repositoryMock.Verify(x => x.FetchAsync<ScoreMap>(It.IsAny<GetScoreMapBetTypesCriteria>(), string.Empty), Times.Once);
        Assert.Equal(3, result1.Count);
        Assert.Equal(3, result2.Count);
    }
}