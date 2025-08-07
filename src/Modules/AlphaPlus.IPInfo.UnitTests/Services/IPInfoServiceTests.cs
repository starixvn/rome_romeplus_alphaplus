using AlphaPlus.IPInfo.DataAccess;
using AlphaPlus.IPInfo.Models;
using AlphaPlus.IPInfo.Services;
using AutoFixture;
using Fanex.Caching;
using Moq;

namespace AlphaPlus.IPInfo.UnitTests.Services;

public class IPInfoServiceTests
{
    private readonly Mock<IIPInfoDataAccess> _mockDataAccess;
    private readonly Mock<ICacheService> _mockCommonCacheService;
    private readonly Mock<ICacheService> _mockIndividualCacheService;
    private readonly IPInformationConfig _config;
    private readonly Fixture _fixture;
    private IPInfoService _service;

    public IPInfoServiceTests()
    {
        _mockDataAccess = new Mock<IIPInfoDataAccess>();
        _mockCommonCacheService = new Mock<ICacheService>();
        _mockIndividualCacheService = new Mock<ICacheService>();
        _fixture = new Fixture();

        _config = new IPInformationConfig
        {
            IPV4TopCountries = "10",
            IPV4TopCountriesCacheInMinutes = 60,
            CommonIPInfoRedisConfiguration = null,
            IndividualIPInfoRedisConfiguration = null
        };

        _service = new IPInfoService(_config, _mockDataAccess.Object);
    }

    [Fact]
    public async Task GetCountries_WithValidIPV4Addresses_ShouldReturnCountries()
    {
        // Arrange
        var ipList = new List<string> { "192.168.1.1", "10.0.0.1" };
        var expectedResult = new Dictionary<string, string>
        {
            { "192.168.1.1", "United States" },
            { "10.0.0.1", "Canada" }
        };

        var ipRanges = new List<IPRange>
        {
            new(3232235777, 3232235777, "United States"), // 192.168.1.1
            new(167772161, 167772161, "Canada") // 10.0.0.1
        };

        _mockDataAccess.Setup(x => x.GetIPV4InfoByTopCountries(It.IsAny<string>()))
            .ReturnsAsync(new List<IPV4Information>
            {
                new() { IPFrom = 3232235777, IPTo = 3232235777, CountryLong = "United States" },
                new() { IPFrom = 167772161, IPTo = 167772161, CountryLong = "Canada" }
            });

        // Act
        var result = await _service.GetCountries(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Count, result.Count);
        Assert.Equal(expectedResult["192.168.1.1"], result["192.168.1.1"]);
        Assert.Equal(expectedResult["10.0.0.1"], result["10.0.0.1"]);
    }

    [Fact]
    public async Task GetCountries_WithEmptyIPList_ShouldReturnEmptyDictionary()
    {
        // Arrange
        var ipList = new List<string>();

        // Act
        var result = await _service.GetCountries(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCountries_WithInvalidIPAddresses_ShouldReturnEmptyDictionary()
    {
        // Arrange
        var ipList = new List<string> { "invalid.ip", "not.an.ip.address" };

        // Act
        var result = await _service.GetCountries(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCountries_WithMixedValidAndInvalidIPs_ShouldReturnOnlyValidResults()
    {
        // Arrange
        var ipList = new List<string> { "192.168.1.1", "invalid.ip", "10.0.0.1" };
        var expectedResult = new Dictionary<string, string>
        {
            { "192.168.1.1", "United States" },
            { "10.0.0.1", "Canada" }
        };

        _mockDataAccess.Setup(x => x.GetIPV4InfoByTopCountries(It.IsAny<string>()))
            .ReturnsAsync(new List<IPV4Information>
            {
                new() { IPFrom = 3232235777, IPTo = 3232235777, CountryLong = "United States" },
                new() { IPFrom = 167772161, IPTo = 167772161, CountryLong = "Canada" }
            });

        // Act
        var result = await _service.GetCountries(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Count, result.Count);
        Assert.Equal(expectedResult["192.168.1.1"], result["192.168.1.1"]);
        Assert.Equal(expectedResult["10.0.0.1"], result["10.0.0.1"]);
    }
}