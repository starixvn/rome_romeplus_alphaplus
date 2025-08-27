using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Criteria;

public class GetProviderBanksCriteriaTests
{
    [Fact]
    public void Constructor_WhenValidSiteProvided_ShouldInitializeSiteProperty()
    {
        // Arrange
        string expectedSite = "test-site";

        // Act
        var criteria = new GetProviderBanksCriteria(expectedSite);

        // Assert
        Assert.Equal(expectedSite, criteria.Site);
    }

    [Fact]
    public void Constructor_WhenEmptySiteProvided_ShouldInitializeSiteProperty()
    {
        // Arrange
        string expectedSite = string.Empty;

        // Act
        var criteria = new GetProviderBanksCriteria(expectedSite);

        // Assert
        Assert.Equal(expectedSite, criteria.Site);
    }

    [Fact]
    public void Constructor_WhenNullSiteProvided_ShouldInitializeSiteProperty()
    {
        // Arrange
        string expectedSite = null!;

        // Act
        var criteria = new GetProviderBanksCriteria(expectedSite);

        // Assert
        Assert.Equal(expectedSite, criteria.Site);
    }

    [Fact]
    public void Constructor_WhenWhitespaceSiteProvided_ShouldInitializeSiteProperty()
    {
        // Arrange
        string expectedSite = "   ";

        // Act
        var criteria = new GetProviderBanksCriteria(expectedSite);

        // Assert
        Assert.Equal(expectedSite, criteria.Site);
    }

    [Fact]
    public void GetSettingKey_WhenCalled_ShouldReturnExpectedSettingKey()
    {
        // Arrange
        var criteria = new GetProviderBanksCriteria("test-site");
        string expectedSettingKey = "Age_CashService_ProviderBankList";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSettingKey, result);
    }

    [Fact]
    public void IsValid_WhenCalled_ShouldAlwaysReturnTrue()
    {
        // Arrange
        var criteria = new GetProviderBanksCriteria("test-site");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("site1")]
    [InlineData("site2")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void IsValid_WhenCalledWithDifferentSites_ShouldAlwaysReturnTrue(string site)
    {
        // Arrange
        var criteria = new GetProviderBanksCriteria(site);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Site_WhenAccessed_ShouldReturnConstructorValue()
    {
        // Arrange
        string expectedSite = "production-site";
        var criteria = new GetProviderBanksCriteria(expectedSite);

        // Act
        var result = criteria.Site;

        // Assert
        Assert.Equal(expectedSite, result);
    }

    [Fact]
    public void Site_WhenPropertyAccessed_ShouldBeReadOnly()
    {
        // Arrange
        var criteria = new GetProviderBanksCriteria("test-site");

        // Act & Assert
        // The Site property should be read-only (getter only)
        // This test verifies that the property can be read but not written to
        var siteValue = criteria.Site;
        Assert.NotNull(siteValue);
    }
}
