using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetScoreMapResultMasterCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int masterId = 123;
        string matchIds = "1,2,3";
        string liveBetTypes = "L1,L2";
        string deadBetTypes = "D1,D2";

        // Act
        var criteria = new GetScoreMapResultMasterCriteria(masterId, matchIds, liveBetTypes, deadBetTypes);

        // Assert
        Assert.Equal(masterId, criteria.MRecommend);
        Assert.Equal(matchIds, criteria.MatchIds);
        Assert.Equal(liveBetTypes, criteria.BetTypes_Live);
        Assert.Equal(deadBetTypes, criteria.BetTypes_Dead);
    }

    [Fact]
    public void GetSettingKey_Should_Return_CorrectKey()
    {
        // Arrange
        var criteria = new GetScoreMapResultMasterCriteria(1, "1", "L", "D");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_ScoreMap_Result_MAg", result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_AllParametersAreValid()
    {
        // Arrange
        var criteria = new GetScoreMapResultMasterCriteria(123, "1,2,3", "L1,L2", "D1,D2");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_MasterIdIsInvalid()
    {
        // Arrange
        var criteria = new GetScoreMapResultMasterCriteria(0, "1,2,3", "L1,L2", "D1,D2");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_Should_Return_False_When_MatchIdsIsInvalid(string matchIds)
    {
        // Arrange
        var criteria = new GetScoreMapResultMasterCriteria(123, matchIds, "L1,L2", "D1,D2");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}