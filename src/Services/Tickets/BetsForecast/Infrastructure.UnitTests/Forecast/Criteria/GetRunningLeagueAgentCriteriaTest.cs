using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetRunningLeagueAgentCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        int agentId = 123;
        string betTypes = "1X2,AH";

        // Act
        var criteria = new GetRunningLeagueAgentCriteria(agentId, betTypes);

        // Assert
        Assert.Equal(agentId, criteria.AgId);
        Assert.Equal(betTypes, criteria.BetTypes);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetRunningLeagueAgentCriteria(1, "1X2");

        // Act
        string key = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_LeagueSelRunning_Ag", key);
    }

    [Theory]
    [InlineData(1, "1X2", true)]
    [InlineData(0, "1X2", false)]
    [InlineData(-1, "1X2", false)]
    [InlineData(1, "", false)]
    [InlineData(1, null, false)]
    [InlineData(1, "   ", false)]
    public void IsValid_ShouldReturnExpectedResult(int agentId, string betTypes, bool expectedResult)
    {
        // Arrange
        var criteria = new GetRunningLeagueAgentCriteria(agentId, betTypes);

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
