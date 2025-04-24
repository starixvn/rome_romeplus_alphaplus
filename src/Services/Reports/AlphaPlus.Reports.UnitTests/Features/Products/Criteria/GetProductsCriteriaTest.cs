using AlphaPlus.Reports.Features.Products;

namespace AlphaPlus.Reports.UnitTests.Features.Products;

[Trait("Category", "Products")]
public class GetProductsCriteriaTest
{
    [Theory]
    [InlineData("SiteA", 1, 1, true)]
    [InlineData("SiteB", 2, 0, true)]
    [InlineData("", 1, 1, false)]
    [InlineData(null, 1, 1, false)]
    [InlineData("SiteA", 0, 1, false)]
    [InlineData("SiteA", -1, 1, false)]
    public void IsValid_ShouldReturnExpectedResult(string site, int currencyId, byte settingStatus, bool expected)
    {
        // Arrange
        var criteria = new GetProductsCriteria(site, currencyId, settingStatus);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetProductsCriteria("SiteA", 1, 1);
        var expected = "Age_GetOfferedProductBySiteCurrency";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var site = "SiteA";
        var currencyId = 1;
        var settingStatus = (byte)1;

        // Act
        var criteria = new GetProductsCriteria(site, currencyId, settingStatus);

        // Assert
        Assert.Equal(site, criteria.Site);
        Assert.Equal(currencyId, criteria.Currency);
        Assert.Equal(settingStatus, criteria.SettingStatus);
    }
}