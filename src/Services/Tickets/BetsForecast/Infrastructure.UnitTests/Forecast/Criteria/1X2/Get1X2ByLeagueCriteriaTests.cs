using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class Get1X2ByLeagueCriteriaTests
{
    [Theory]
    [InlineData(4, "Age_Forecast1x2_SelByLeague_SAg")] // Super level
    public void GetSettingKey_ForSuperLevel_ReturnsCorrectKey(byte custLevel, string expectedKey)
    {
        // Arrange
        var criteria = new Get1X2ByLeagueCriteria(custLevel, 123, 456);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(3, "Age_Forecast1x2_SelByLeague_MAg")] // Master level
    public void GetSettingKey_ForMasterLevel_ReturnsCorrectKey(byte custLevel, string expectedKey)
    {
        // Arrange
        var criteria = new Get1X2ByLeagueCriteria(custLevel, 123, 456);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(2, "Age_Forecast1x2_SelByLeague_Ag")] // Agent level
    [InlineData(1, "Age_Forecast1x2_SelByLeague_Ag")] // Member level (should use same key as Agent)
    public void GetSettingKey_ForAgentOrMemberLevel_ReturnsCorrectKey(byte custLevel, string expectedKey)
    {
        // Arrange
        var criteria = new Get1X2ByLeagueCriteria(custLevel, 123, 456);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(1, 1, true)]  // Valid values
    [InlineData(0, 1, false)] // Invalid custId
    [InlineData(1, 0, false)] // Invalid leagueId
    [InlineData(0, 0, false)] // Both invalid
    public void IsValid_ValidatesParameters_Correctly(int custId, int leagueId, bool expectedResult)
    {
        // Arrange
        var criteria = new Get1X2ByLeagueCriteria(2, custId, leagueId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        const byte custLevel = 4;
        const int custId = 123;
        const int leagueId = 456;
        var criteria = new Get1X2ByLeagueCriteria(custLevel, custId, leagueId);

        // Assert
        Assert.Equal(UserLevel.Super, criteria.CustLevel);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(leagueId, criteria.LeagueId);
    }
}