using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetMatchCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int expectedMatchId = 123;
        int expectedBetType = 456;

        // Act
        var criteria = new GetMatchCriteria(expectedMatchId, expectedBetType);

        // Assert
        Assert.Equal(expectedMatchId, criteria.MatchId);
        Assert.Equal(expectedBetType, criteria.BetType);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Value()
    {
        // Arrange
        var criteria = new GetMatchCriteria(1, 1);
        string expectedKey = "Age_Forecast_OddsSelTopByMatch";

        // Act
        string actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    [InlineData(-1, 1, false)]
    [InlineData(1, -1, false)]
    public void IsValid_Should_Return_Expected_Result(int matchId, int betType, bool expectedResult)
    {
        // Arrange
        var criteria = new GetMatchCriteria(matchId, betType);

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}