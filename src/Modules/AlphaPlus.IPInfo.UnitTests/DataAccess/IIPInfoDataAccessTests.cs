using AlphaPlus.IPInfo.DataAccess;
using AlphaPlus.IPInfo.Models;

namespace AlphaPlus.IPInfo.UnitTests.DataAccess;

public class IIPInfoDataAccessTests
{
    private readonly Mock<IIPInfoDataAccess> _mockDataAccess;

    public IIPInfoDataAccessTests()
    {
        _mockDataAccess = new Mock<IIPInfoDataAccess>();
    }

    [Fact]
    public async Task GetIPV4InfoByTopCountries_ShouldReturnExpectedData()
    {
        // Arrange
        var countries = "US,CA,GB";
        var expectedData = new List<IPV4Information>
        {
            new() { IPFrom = 3232235777, IPTo = 3232235777, CountryLong = "United States" },
            new() { IPFrom = 167772161, IPTo = 167772161, CountryLong = "Canada" }
        };

        _mockDataAccess.Setup(x => x.GetIPV4InfoByTopCountries(countries))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockDataAccess.Object.GetIPV4InfoByTopCountries(countries);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedData.Count, result.Count());
        Assert.Equal(expectedData[0].CountryLong, result.First().CountryLong);
        Assert.Equal(expectedData[1].CountryLong, result.Last().CountryLong);
    }

    [Fact]
    public async Task GetIPV4Info_ShouldReturnExpectedData()
    {
        // Arrange
        var ipList = "192.168.1.1,10.0.0.1";
        var expectedData = new List<IPV4Information>
        {
            new() { IPFrom = 3232235777, IPTo = 3232235777, CountryLong = "United States" },
            new() { IPFrom = 167772161, IPTo = 167772161, CountryLong = "Canada" }
        };

        _mockDataAccess.Setup(x => x.GetIPV4Info(ipList))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockDataAccess.Object.GetIPV4Info(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedData.Count, result.Count());
        Assert.Equal(expectedData[0].CountryLong, result.First().CountryLong);
        Assert.Equal(expectedData[1].CountryLong, result.Last().CountryLong);
    }

    [Fact]
    public async Task GetIPV6Info_ShouldReturnExpectedData()
    {
        // Arrange
        var ipList = "2001:db8::1,::1";
        var expectedData = new List<IPV6Information>
        {
            new() { IPFrom = new byte[16], IPTo = new byte[16], CountryLong = "United States" },
            new() { IPFrom = new byte[16], IPTo = new byte[16], CountryLong = "Canada" }
        };

        _mockDataAccess.Setup(x => x.GetIPV6Info(ipList))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockDataAccess.Object.GetIPV6Info(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedData.Count, result.Count());
        Assert.Equal(expectedData[0].CountryLong, result.First().CountryLong);
        Assert.Equal(expectedData[1].CountryLong, result.Last().CountryLong);
    }

    [Fact]
    public async Task GetIPV4InfoByTopCountries_WithEmptyCountries_ShouldReturnEmptyList()
    {
        // Arrange
        var countries = "";
        var expectedData = new List<IPV4Information>();

        _mockDataAccess.Setup(x => x.GetIPV4InfoByTopCountries(countries))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockDataAccess.Object.GetIPV4InfoByTopCountries(countries);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetIPV4Info_WithEmptyIPList_ShouldReturnEmptyList()
    {
        // Arrange
        var ipList = "";
        var expectedData = new List<IPV4Information>();

        _mockDataAccess.Setup(x => x.GetIPV4Info(ipList))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockDataAccess.Object.GetIPV4Info(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetIPV6Info_WithEmptyIPList_ShouldReturnEmptyList()
    {
        // Arrange
        var ipList = "";
        var expectedData = new List<IPV6Information>();

        _mockDataAccess.Setup(x => x.GetIPV6Info(ipList))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _mockDataAccess.Object.GetIPV6Info(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}