using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.DataAccesses;
using AlphaPlus.IPInfo.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.UnitTests.Shared.DataAccesses;

[Trait("Category", "BetLists")]
public class IPInfoDataAccessTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly IPInfoDataAccess _dataAccess;

    public IPInfoDataAccessTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _dataAccess = new IPInfoDataAccess(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetIPV4Info_ShouldCallRepositoryWithCorrectCriteria()
    {
        // Arrange
        var ipList = "192.168.1.1,10.0.0.1";
        var expectedResult = new List<IPV4Information>
        {
            new IPV4Information { IP = "192.168.1.1", CountryShort = "US" },
            new IPV4Information { IP = "10.0.0.1", CountryShort = "GB" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<IPV4Information>(It.IsAny<GetIPV4InfoCriteria>(), string.Empty))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _dataAccess.GetIPV4Info(ipList);

        // Assert
        _repositoryMock.Verify(r => r.FetchAsync<IPV4Information>(It.Is<GetIPV4InfoCriteria>(c => c.IPList == ipList), string.Empty), Times.Once);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetIPV4InfoByTopCountries_ShouldCallRepositoryWithCorrectCriteria()
    {
        // Arrange
        var countries = "US,GB";
        var expectedResult = new List<IPV4Information>
        {
            new IPV4Information { IP = "192.168.1.1", CountryShort = "US" },
            new IPV4Information { IP = "10.0.0.1", CountryShort = "GB" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<IPV4Information>(It.IsAny<GetIPV4InfoByTopCountryCriteria>(), string.Empty))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _dataAccess.GetIPV4InfoByTopCountries(countries);

        // Assert
        _repositoryMock.Verify(r => r.FetchAsync<IPV4Information>(It.Is<GetIPV4InfoByTopCountryCriteria>(c => c.CountryCodes == countries), string.Empty), Times.Once);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetIPV6Info_ShouldCallRepositoryWithCorrectCriteria()
    {
        // Arrange
        var ipList = "2001:db8::1,2001:db8::2";
        var expectedResult = new List<IPV6Information>
        {
            new IPV6Information { IP = "2001:db8::1", CountryShort = "US" },
            new IPV6Information { IP = "2001:db8::2", CountryShort = "GB" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<IPV6Information>(It.IsAny<GetIPV6InfoCriteria>(), string.Empty))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _dataAccess.GetIPV6Info(ipList);

        // Assert
        _repositoryMock.Verify(r => r.FetchAsync<IPV6Information>(It.Is<GetIPV6InfoCriteria>(c => c.IPList == ipList), string.Empty), Times.Once);
        Assert.Equal(expectedResult, result);
    }
}