using AlphaPlus.CashService.API.Criteria.Common;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Common;

public class GetCountryCodesCriteriaTests
{
    [Fact]
    public void Constructor_WhenGivenSiteName_SetsSiteNameProperty()
    {
        // Arrange
        const string expectedSiteName = "TestSite";

        // Act
        var criteria = new GetCountryCodesCriteria(expectedSiteName);

        // Assert
        Assert.Equal(expectedSiteName, criteria.SiteName);
    }

    [Fact]
    public void GetSettingKey_WhenCalled_ReturnsAgeCashServiceCountryCodeSel()
    {
        // Arrange
        var criteria = new GetCountryCodesCriteria("TestSite");
        const string expectedKey = "Age_CashService_CountryCodeSel";

        // Act
        var actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Fact]
    public void IsValid_WhenCalled_ReturnsTrue()
    {
        // Arrange
        var criteria = new GetCountryCodesCriteria("TestSite");

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }
} 