using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetScoreMapResultSuperCriteriaTests
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange & Act
        var superId = 123;
        var matchIds = "1,2,3";
        var liveBetTypes = "L1,L2";
        var deadBetTypes = "D1,D2";
        var criteria = new GetScoreMapResultSuperCriteria(superId, matchIds, liveBetTypes, deadBetTypes);

        // Assert
        Assert.Equal(superId, criteria.SRecommend);
        Assert.Equal(matchIds, criteria.MatchIds);
        Assert.Equal(liveBetTypes, criteria.BetTypes_Live);
        Assert.Equal(deadBetTypes, criteria.BetTypes_Dead);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Expected_Value()
    {
        // Arrange
        var criteria = new GetScoreMapResultSuperCriteria(1, "1", "L1", "D1");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_ScoreMap_Result_SAg", result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_All_Properties_Are_Valid()
    {
        // Arrange
        var criteria = new GetScoreMapResultSuperCriteria(123, "1,2,3", "L1,L2", "D1,D2");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_SuperId_Is_Zero()
    {
        // Arrange
        var criteria = new GetScoreMapResultSuperCriteria(0, "1,2,3", "L1,L2", "D1,D2");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_SuperId_Is_Negative()
    {
        // Arrange
        var criteria = new GetScoreMapResultSuperCriteria(-1, "1,2,3", "L1,L2", "D1,D2");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_MatchIds_Is_Null_Or_Empty()
    {
        // Arrange
        var criteria1 = new GetScoreMapResultSuperCriteria(123, null, "L1,L2", "D1,D2");
        var criteria2 = new GetScoreMapResultSuperCriteria(123, "", "L1,L2", "D1,D2");

        // Act
        var result1 = criteria1.IsValid();
        var result2 = criteria2.IsValid();

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Theory]
    [InlineData(0, "", "L1", "D1", false)]
    [InlineData(1, "", "L1", "D1", false)]
    [InlineData(0, "1", "L1", "D1", false)]
    [InlineData(1, "1", "L1", "D1", true)]
    public void IsValid_Should_Return_Expected_Results_For_Different_Inputs(
        int superId,
        string matchIds,
        string liveBetTypes,
        string deadBetTypes,
        bool expected)
    {
        // Arrange
        var criteria = new GetScoreMapResultSuperCriteria(superId, matchIds, liveBetTypes, deadBetTypes);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}