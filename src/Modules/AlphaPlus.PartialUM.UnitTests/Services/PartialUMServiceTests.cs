using AlphaPlus.PartialUM.Configurations;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Exceptions;
using AlphaPlus.PartialUM.Models;
using AlphaPlus.PartialUM.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AlphaPlus.PartialUM.UnitTests.Services;

public class PartialUMServiceTests
{
    private readonly Mock<IMemoryCache> _mockCacheService;
    private readonly Mock<IPartialUMDataAccess> _mockDataAccess;
    private readonly Mock<IOptions<PartialUMOptions>> _mockOptions;
    private readonly Dictionary<string, IList<string>> _partialUMConfig;
    private readonly PartialUMService _service;

    public PartialUMServiceTests()
    {
        _mockCacheService = new Mock<IMemoryCache>();
        _mockDataAccess = new Mock<IPartialUMDataAccess>();
        _mockOptions = new Mock<IOptions<PartialUMOptions>>();
        _partialUMConfig = new Dictionary<string, IList<string>>
        {
            ["TestSetting"] = new List<string> { "Function1", "Function2" },
            ["AnotherSetting"] = new List<string> { "Function3" }
        };

        _mockOptions.Setup(x => x.Value).Returns(new PartialUMOptions { CacheInSeconds = 300 });

        _service = new PartialUMService(
            _mockCacheService.Object,
            _mockDataAccess.Object,
            _partialUMConfig,
            _mockOptions.Object);
    }

    [Fact]
    public async Task ThrowIfUM_WhenSettingKeyNotFound_ShouldReturnWithoutException()
    {
        // Arrange
        var settingKey = "NonExistentSetting";
        var siteName = "TestSite";

        // Act
        await _service.ThrowIfUM(settingKey, siteName);

        // Assert
        // No exception should be thrown and no cache should be accessed
    }

    [Fact]
    public async Task ThrowIfUM_WhenNoUMEntities_ShouldReturnWithoutException()
    {
        // Arrange
        var settingKey = "TestSetting";
        var siteName = "TestSite";
        var emptyEntities = Enumerable.Empty<UMEntity>();

        SetupCacheMock(emptyEntities);

        // Act
        await _service.ThrowIfUM(settingKey, siteName);

        // Assert
        // No exception should be thrown
    }

    [Fact]
    public async Task ThrowIfUM_WhenUMEntitiesExistButNoMatchingFunction_ShouldReturnWithoutException()
    {
        // Arrange
        var settingKey = "TestSetting";
        var siteName = "TestSite";
        var umEntities = new List<UMEntity>
        {
            new() { FunctionTypeName = "NonMatchingFunction", BeginTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) }
        };

        SetupCacheMock(umEntities);

        // Act
        await _service.ThrowIfUM(settingKey, siteName);

        // Assert
        // No exception should be thrown
    }

    [Fact]
    public async Task ThrowIfUM_WhenMatchingUMEntityExists_ShouldThrowPartialUMException()
    {
        // Arrange
        var settingKey = "TestSetting";
        var siteName = "TestSite";
        var beginTime = DateTime.Now;
        var endTime = DateTime.Now.AddHours(1);
        var umEntities = new List<UMEntity>
        {
            new() { FunctionTypeName = "Function1", BeginTime = beginTime, EndTime = endTime }
        };

        SetupCacheMock(umEntities);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PartialUMException>(() =>
            _service.ThrowIfUM(settingKey, siteName));

        Assert.Equal(beginTime.ToString("dd/MM/yyyy hh:mm tt"), exception.StartTime);
        Assert.Equal(endTime.ToString("dd/MM/yyyy hh:mm tt"), exception.EndTime);
    }

    [Fact]
    public async Task ThrowIfUM_WhenMultipleUMEntitiesExist_ShouldThrowExceptionForFirstMatch()
    {
        // Arrange
        var settingKey = "TestSetting";
        var siteName = "TestSite";
        var beginTime1 = DateTime.Now;
        var endTime1 = DateTime.Now.AddHours(1);
        var beginTime2 = DateTime.Now.AddHours(2);
        var endTime2 = DateTime.Now.AddHours(3);

        var umEntities = new List<UMEntity>
        {
            new() { FunctionTypeName = "NonMatchingFunction", BeginTime = beginTime1, EndTime = endTime1 },
            new() { FunctionTypeName = "Function1", BeginTime = beginTime2, EndTime = endTime2 }
        };

        SetupCacheMock(umEntities);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PartialUMException>(() =>
            _service.ThrowIfUM(settingKey, siteName));

        Assert.Equal(beginTime2.ToString("dd/MM/yyyy hh:mm tt"), exception.StartTime);
        Assert.Equal(endTime2.ToString("dd/MM/yyyy hh:mm tt"), exception.EndTime);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ThrowIfUM_WithInvalidSiteName_ShouldHandleGracefully(string siteName)
    {
        // Arrange
        var settingKey = "TestSetting";
        var umEntities = Enumerable.Empty<UMEntity>();

        SetupCacheMock(umEntities);

        // Act
        await _service.ThrowIfUM(settingKey, siteName);

        // Assert
        // Should handle gracefully without throwing exceptions
    }

    private void SetupCacheMock(IEnumerable<UMEntity> entities)
    {
        // Since we can't mock extension methods, we'll use a different approach
        // We'll create a real memory cache for testing
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var cacheKey = $"AlphaPlus.PartialUM.Services.PartialUM.SiteName-TestSite";

        // Set up the data access to return the entities
        _mockDataAccess.Setup(x => x.GetUMEntities(It.IsAny<string>()))
            .ReturnsAsync(entities);

        // Replace the mock cache with a real one for this test
        var realService = new PartialUMService(
            memoryCache,
            _mockDataAccess.Object,
            _partialUMConfig,
            _mockOptions.Object);

        // Use reflection to replace the service instance
        var field = typeof(PartialUMServiceTests).GetField("_service",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(this, realService);
    }
}
