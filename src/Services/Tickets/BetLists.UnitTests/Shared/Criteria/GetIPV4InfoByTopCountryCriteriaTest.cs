using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetIPV4InfoByTopCountryCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeCountryCodesProperty()
    {
        // Arrange & Act
        const string expectedCountryCodes = "US,UK,CA";
        var criteria = new GetIPV4InfoByTopCountryCriteria(expectedCountryCodes);

        // Assert
        Assert.Equal(expectedCountryCodes, criteria.CountryCodes);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetIPV4InfoByTopCountryCriteria("US,UK,CA");
        var expectedKey = "Age_IPRangesByCountrySel";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCountryCodesIsNotEmpty()
    {
        // Arrange
        var criteria = new GetIPV4InfoByTopCountryCriteria("US,UK,CA");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_ShouldReturnFalse_WhenCountryCodesIsNullOrWhiteSpace(string countryCodes)
    {
        // Arrange
        var criteria = new GetIPV4InfoByTopCountryCriteria(countryCodes);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}