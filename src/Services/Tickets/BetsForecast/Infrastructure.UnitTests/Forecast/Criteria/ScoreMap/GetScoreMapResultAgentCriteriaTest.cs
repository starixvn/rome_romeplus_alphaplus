using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetScoreMapResultAgentCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int agentId = 123;
        string matchIds = "1,2,3";
        string liveBetTypes = "type1,type2";
        string deadBetTypes = "deadType1,deadType2";

        // Act
        var criteria = new GetScoreMapResultAgentCriteria(agentId, matchIds, liveBetTypes, deadBetTypes);

        // Assert
        Assert.Equal(agentId, criteria.Recommend);
        Assert.Equal(matchIds, criteria.MatchIds);
        Assert.Equal(liveBetTypes, criteria.BetTypes_Live);
        Assert.Equal(deadBetTypes, criteria.BetTypes_Dead);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetScoreMapResultAgentCriteria(1, "1", "live", "dead");

        // Act
        var key = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_ScoreMap_Result_Ag", key);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenAllConditionsAreMet()
    {
        // Arrange
        var criteria = new GetScoreMapResultAgentCriteria(1, "1", "live", "dead");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenRecommendIsZero()
    {
        // Arrange
        var criteria = new GetScoreMapResultAgentCriteria(0, "1", "live", "dead");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenRecommendIsNegative()
    {
        // Arrange
        var criteria = new GetScoreMapResultAgentCriteria(-1, "1", "live", "dead");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenMatchIdsIsEmpty()
    {
        // Arrange
        var criteria = new GetScoreMapResultAgentCriteria(1, "", "live", "dead");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenAllConditionsAreInvalid()
    {
        // Arrange
        var criteria = new GetScoreMapResultAgentCriteria(0, "", "", "");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}
