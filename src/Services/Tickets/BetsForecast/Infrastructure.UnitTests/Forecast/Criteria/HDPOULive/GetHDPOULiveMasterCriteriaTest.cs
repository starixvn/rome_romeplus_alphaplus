using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "HDPOULiveCriteria")]
public class GetHDPOULiveMasterCriteriaTest
{
    [Theory]
    [InlineData(1, 10, true)]
    [InlineData(123, 456, true)]
    [InlineData(0, 10, false)]
    [InlineData(-1, 10, false)]
    [InlineData(1, 0, false)]
    [InlineData(1, -1, false)]
    [InlineData(0, 0, false)]
    public void IsValid_Should_Return_Expected_Result(int masterId, int leagueId, bool expected)
    {
        // Arrange
        var criteria = new GetHDPOULiveMasterCriteria(masterId, leagueId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(123, 456)]
    public void Constructor_Should_Initialize_Properties_Correctly(int masterId, int leagueId)
    {
        // Arrange & Act
        var criteria = new GetHDPOULiveMasterCriteria(masterId, leagueId);

        // Assert
        Assert.Equal(masterId, criteria.MRecommend);
        Assert.Equal(leagueId, criteria.LeagueId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Expected_Value()
    {
        // Arrange
        var criteria = new GetHDPOULiveMasterCriteria(1, 10);
        const string expectedKey = "Age_Forecast_SelByLeague_MAg";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }
}