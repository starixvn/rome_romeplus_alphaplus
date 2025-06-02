using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetHDPOULiveAgentCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithExpectedValues()
    {
        // Arrange
        int expectedAgentId = 123;
        int expectedLeagueId = 456;

        // Act
        var criteria = new GetHDPOULiveAgentCriteria(expectedAgentId, expectedLeagueId);

        // Assert
        Assert.Equal(expectedAgentId, criteria.Recommend);
        Assert.Equal(expectedLeagueId, criteria.LeagueId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturn_CorrectKey()
    {
        // Arrange
        var criteria = new GetHDPOULiveAgentCriteria(1, 1);
        string expectedKey = "Age_Forecast_SelByLeague_Ag";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(-1, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(1, -1, false)]
    [InlineData(0, 0, false)]
    public void IsValid_ShouldReturn_ExpectedResult(int agentId, int leagueId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetHDPOULiveAgentCriteria(agentId, leagueId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}