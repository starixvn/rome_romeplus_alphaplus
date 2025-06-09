using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Criteria;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace AgencyCash.API.Tests.Shared;

[Trait("Category", "Shared")]
public class SiteDBServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private readonly SiteDBService _service;
    private readonly Mock<ICacheEntry> _mockCacheEntry;

    public SiteDBServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockMemoryCache = new Mock<IMemoryCache>();
        _mockCacheEntry = new Mock<ICacheEntry>();

        _mockMemoryCache
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(_mockCacheEntry.Object);

        _service = new SiteDBService(_mockRepository.Object, _mockMemoryCache.Object);
    }

    [Fact]
    public async Task GetCustDBGroupId_ShouldReturnCustDBGroupId_WhenConfigExists()
    {
        // Arrange
        const int custId = 123;
        const int expectedCustDBGroupId = 456;

        var config = new SiteDBConfig
        {
            CustDBGroupId = expectedCustDBGroupId,
            SiteDBGroupId = 789
        };

        _mockRepository
            .Setup(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default))
            .ReturnsAsync(config);

        // Act
        var result = await _service.GetCustDBGroupId(custId);

        // Assert
        Assert.Equal(expectedCustDBGroupId, result);
    }

    [Fact]
    public async Task GetCustDBGroupId_ShouldReturnZero_WhenConfigDoesNotExist()
    {
        // Arrange
        const int custId = 123;

        _mockRepository
            .Setup(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _service.GetCustDBGroupId(custId);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetSiteDBGroupId_ShouldReturnSiteDBGroupId_WhenConfigExists()
    {
        // Arrange
        const int custId = 123;
        const int expectedSiteDBGroupId = 789;

        var config = new SiteDBConfig
        {
            CustDBGroupId = 456,
            SiteDBGroupId = expectedSiteDBGroupId
        };

        _mockRepository
            .Setup(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default))
            .ReturnsAsync(config);

        // Act
        var result = await _service.GetSiteDBGroupId(custId);

        // Assert
        Assert.Equal(expectedSiteDBGroupId, result);
    }

    [Fact]
    public async Task GetSiteDBGroupId_ShouldReturnZero_WhenConfigDoesNotExist()
    {
        // Arrange
        const int custId = 123;

        _mockRepository
            .Setup(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _service.GetSiteDBGroupId(custId);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetSiteDBConfig_ShouldGetFromCache_WhenCacheHasValue()
    {
        // Arrange
        const int custId = 123;
        var cacheKey = $"AgencyCash.API.SiteDBConfig_{custId}";
        var expectedConfig = new SiteDBConfig
        {
            CustDBGroupId = 456,
            SiteDBGroupId = 789
        };

        object value = expectedConfig;
        _mockMemoryCache
            .Setup(x => x.TryGetValue(cacheKey, out value))
            .Returns(true);

        // Act
        var result = await _service.GetSiteDBConfig(custId);

        // Assert
        Assert.Same(expectedConfig, result);
        _mockRepository.Verify(r => r.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), string.Empty, default), Times.Never);
    }

    [Fact]
    public async Task GetSiteDBConfig_ShouldGetFromRepository_WhenCacheDoesNotHaveValue()
    {
        // Arrange
        const int custId = 123;
        var cacheKey = $"AgencyCash.API.SiteDBConfig_{custId}";
        var expectedConfig = new SiteDBConfig
        {
            CustDBGroupId = 456,
            SiteDBGroupId = 789
        };

        object value = null;
        _mockMemoryCache
            .Setup(x => x.TryGetValue(cacheKey, out value))
            .Returns(false);

        _mockRepository
            .Setup(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _service.GetSiteDBConfig(custId);

        // Assert
        Assert.Same(expectedConfig, result);
        _mockRepository.Verify(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default), Times.Once);
        _mockMemoryCache.Verify(m => m.CreateEntry(cacheKey), Times.Once);
    }

    [Fact]
    public async Task GetSiteDBConfig_ShouldNotCacheNull_WhenRepositoryReturnsNull()
    {
        // Arrange
        const int custId = 123;
        var cacheKey = $"AgencyCash.API.SiteDBConfig_{custId}";

        object value = null;
        _mockMemoryCache
            .Setup(x => x.TryGetValue(cacheKey, out value))
            .Returns(false);

        _mockRepository
            .Setup(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _service.GetSiteDBConfig(custId);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetAsync<SiteDBConfig>(It.Is<GetSiteDBConfigCriteria>(c => c.CustId == custId), string.Empty, default), Times.Once);
        _mockMemoryCache.Verify(m => m.CreateEntry(cacheKey), Times.Never);
    }
}