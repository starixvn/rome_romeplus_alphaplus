using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetHDPOULiveSuperCriteriaTest
{
    [Fact]
    public void Constructor_InitializesProperties_Correctly()
    {
        // Arrange & Act
        const int superId = 5;
        const int leagueId = 10;
        var criteria = new GetHDPOULiveSuperCriteria(superId, leagueId);

        // Assert
        Assert.Equal(superId, criteria.SRecommend);
        Assert.Equal(leagueId, criteria.LeagueId);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectKey()
    {
        // Arrange
        var criteria = new GetHDPOULiveSuperCriteria(1, 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Forecast_SelByLeague_SAg", result);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(-1, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(1, -1, false)]
    [InlineData(0, 0, false)]
    public void IsValid_ValidatesCriteriaCorrectly(int superId, int leagueId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetHDPOULiveSuperCriteria(superId, leagueId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}