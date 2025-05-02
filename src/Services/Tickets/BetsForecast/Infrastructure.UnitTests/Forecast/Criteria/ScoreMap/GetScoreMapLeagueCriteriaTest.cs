using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetScoreMapLeagueCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetAgentIdCorrectly()
    {
        // Arrange
        var custId = 123;

        // Act
        var criteria = new GetScoreMapLeagueCriteria(custId);

        // Assert
        Assert.Equal(custId, criteria.AgentId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetScoreMapLeagueCriteria(1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_ScoreMap_SelLeague", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldReturnExpectedResult(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetScoreMapLeagueCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}
