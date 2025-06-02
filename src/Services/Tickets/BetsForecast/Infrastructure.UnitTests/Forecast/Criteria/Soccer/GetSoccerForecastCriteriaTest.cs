using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetSoccerForecastCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int custId = 123;
        string matchIds = "1,2,3";
        bool isLive = true;

        // Act
        var criteria = new GetSoccerForecastCriteria(custId, matchIds, isLive);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(matchIds, criteria.MatchIdList);
        Assert.Equal(isLive, criteria.IsLive);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetSoccerForecastCriteria(1, "1", false);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_NewTotalBetsForeCast_Main", result);
    }

    [Theory]
    [InlineData(1, "1,2,3", true)]
    [InlineData(123, "456", false)]
    public void IsValid_WithValidParameters_ShouldReturnTrue(int custId, string matchIds, bool isLive)
    {
        // Arrange
        var criteria = new GetSoccerForecastCriteria(custId, matchIds, isLive);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0, "1,2,3", true)]
    [InlineData(-1, "456", false)]
    [InlineData(1, "", true)]
    [InlineData(1, null, false)]
    [InlineData(1, "   ", true)]
    [InlineData(0, "", false)]
    public void IsValid_WithInvalidParameters_ShouldReturnFalse(int custId, string matchIds, bool isLive)
    {
        // Arrange
        var criteria = new GetSoccerForecastCriteria(custId, matchIds, isLive);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}
