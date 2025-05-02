using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetRunningLeagueSuperCriteriaTest
{
    [Fact]
    public void Constructor_SetsProperties_Correctly()
    {
        // Arrange
        int superId = 123;
        string betTypes = "1x2,ou";

        // Act
        var criteria = new GetRunningLeagueSuperCriteria(superId, betTypes);

        // Assert
        Assert.Equal(superId, criteria.SAgId);
        Assert.Equal(betTypes, criteria.BetTypes);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectKey()
    {
        // Arrange
        var criteria = new GetRunningLeagueSuperCriteria(1, "1x2");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_LeagueSelRunning_SAg", result);
    }

    [Theory]
    [InlineData(1, "1x2", true)]
    [InlineData(0, "1x2", false)]
    [InlineData(-1, "1x2", false)]
    [InlineData(1, "", false)]
    [InlineData(1, null, false)]
    [InlineData(1, "   ", false)]
    public void IsValid_ValidatesInputsCorrectly(int superId, string betTypes, bool expected)
    {
        // Arrange
        var criteria = new GetRunningLeagueSuperCriteria(superId, betTypes);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}
