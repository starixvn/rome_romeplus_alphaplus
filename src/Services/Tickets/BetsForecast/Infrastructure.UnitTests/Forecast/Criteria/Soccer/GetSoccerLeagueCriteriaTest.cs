using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetSoccerLeagueCriteriaTest
{
    [Fact]
    public void Constructor_Should_SetProperties_Correctly()
    {
        // Arrange & Act
        var criteria = new GetSoccerLeagueCriteria(isLive: true, isShowAll: false);

        // Assert
        Assert.True(criteria.IsLive);
        Assert.False(criteria.IsShowAll);
    }

    [Fact]
    public void GetSettingKey_Should_Return_CorrectKey()
    {
        // Arrange
        var criteria = new GetSoccerLeagueCriteria(isLive: true, isShowAll: true);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_NewTotalBetsForeCast_LeagueSel", result);
    }

    [Fact]
    public void IsValid_Should_Always_ReturnTrue()
    {
        // Arrange
        var criteria = new GetSoccerLeagueCriteria(isLive: false, isShowAll: false);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}
