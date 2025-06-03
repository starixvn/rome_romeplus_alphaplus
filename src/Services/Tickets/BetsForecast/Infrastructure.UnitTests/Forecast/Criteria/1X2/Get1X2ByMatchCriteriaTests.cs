using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class Get1X2ByMatchCriteriaTests
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange & Act
        byte custLevel = 3;
        int custId = 123;
        int matchId = 456;
        int betType = 5;
        var criteria = new Get1X2ByMatchCriteria(custLevel, custId, matchId, betType);

        // Assert
        Assert.Equal(custLevel, criteria.RoleId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(matchId, criteria.MatchId);
        Assert.Equal(betType, criteria.BetType);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new Get1X2ByMatchCriteria(2, 123, 456, 5);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Forecast_Sel1X2", result);
    }

    [Theory]
    [InlineData(2, 123, 456, 5, true)]  // Valid inputs
    [InlineData(1, 123, 456, 5, false)] // RoleId = 1 (invalid)
    [InlineData(2, 0, 456, 5, false)]   // CustId = 0 (invalid)
    [InlineData(2, 123, 0, 5, false)]   // MatchId = 0 (invalid)
    [InlineData(2, 123, 456, 0, false)] // BetType = 0 (invalid)
    public void IsValid_Should_Return_Correct_Result(byte roleId, int custId, int matchId, int betType, bool expectedResult)
    {
        // Arrange
        var criteria = new Get1X2ByMatchCriteria(roleId, custId, matchId, betType);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}