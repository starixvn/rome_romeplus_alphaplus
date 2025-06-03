using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetSoccerMatchCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int leagueId = 123;
        bool isLive = true;

        // Act
        var criteria = new GetSoccerMatchCriteria(leagueId, isLive);

        // Assert
        Assert.Equal(leagueId, criteria.LeagueId);
        Assert.Equal(isLive, criteria.IsLive);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Value()
    {
        // Arrange
        var criteria = new GetSoccerMatchCriteria(123, false);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_NewTotalBetsForeCast_EventSel", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(int.MaxValue, true)]
    public void IsValid_Should_Return_True_When_LeagueId_IsGreaterThan_Zero(int leagueId, bool expected)
    {
        // Arrange
        var criteria = new GetSoccerMatchCriteria(leagueId, false);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(int.MinValue, false)]
    public void IsValid_Should_Return_False_When_LeagueId_IsNotGreaterThan_Zero(int leagueId, bool expected)
    {
        // Arrange
        var criteria = new GetSoccerMatchCriteria(leagueId, false);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}
