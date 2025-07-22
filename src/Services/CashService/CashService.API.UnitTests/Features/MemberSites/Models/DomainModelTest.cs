using AlphaPlus.CashService.API.Features.MemberSites.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.MemberSites.Models;

public class DomainModelTest
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var domainModel = new DomainModel();

        // Assert
        Assert.Null(domainModel.Url);
    }

    [Fact]
    public void Url_WhenSet_ShouldStoreValueCorrectly()
    {
        // Arrange
        var domainModel = new DomainModel();
        var expectedUrl = "https://example.com";

        // Act
        domainModel.Url = expectedUrl;

        // Assert
        Assert.Equal(expectedUrl, domainModel.Url);
    }

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://test.com")]
    [InlineData("")]
    [InlineData(null)]
    public void Url_WhenSetWithDifferentValues_ShouldStoreValueCorrectly(string url)
    {
        // Arrange
        var domainModel = new DomainModel();

        // Act
        domainModel.Url = url;

        // Assert
        Assert.Equal(url, domainModel.Url);
    }
}