using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetScoreMapMatchCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var leagueId = 456;

        // Act
        var criteria = new GetScoreMapMatchCriteria(custId, leagueId);

        // Assert
        Assert.Equal(custId, criteria.AgentId);
        Assert.Equal(leagueId, criteria.LeagueId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetScoreMapMatchCriteria(123, 456);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_ScoreMap_SelMatch", settingKey);
    }

    [Theory]
    [InlineData(123, 456, true)]
    [InlineData(0, 456, false)]
    [InlineData(123, 0, false)]
    [InlineData(0, 0, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, int leagueId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetScoreMapMatchCriteria(custId, leagueId);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }
}