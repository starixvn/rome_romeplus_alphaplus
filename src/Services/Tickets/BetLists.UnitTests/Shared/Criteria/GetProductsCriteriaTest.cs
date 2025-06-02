using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetProductsCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string site = "test-site";
        const int currencyId = 1;
        const byte settingStatus = 1;

        // Act
        var criteria = new GetProductsCriteria(site, currencyId, settingStatus);

        // Assert
        Assert.Equal(site, criteria.Site);
        Assert.Equal(currencyId, criteria.Currency);
        Assert.Equal(settingStatus, criteria.SettingStatus);
    }

    [Fact]
    public void IsValid_WithValidParameters_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new GetProductsCriteria("test-site", 1, 1);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData(null, 1)]
    [InlineData("", 1)]
    [InlineData(" ", 1)]
    [InlineData("site", 0)]
    [InlineData("site", -1)]
    public void IsValid_WithInvalidParameters_ShouldReturnFalse(string site, int currencyId)
    {
        // Arrange
        var criteria = new GetProductsCriteria(site, currencyId, 1);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetProductsCriteria("test-site", 1, 1);
        const string expectedKey = "Age_GetOfferedProductBySiteCurrency";

        // Act
        var key = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, key);
    }
}