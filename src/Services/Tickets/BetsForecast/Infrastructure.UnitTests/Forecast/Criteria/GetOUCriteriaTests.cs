using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetOUCriteriaTests
{
    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange
        byte custLevel = 2;
        int custId = 123;
        int matchId = 456;
        int betType = 789;

        // Act
        var criteria = new GetOUCriteria(custLevel, custId, matchId, betType);

        // Assert
        Assert.Equal(custLevel, criteria.RoleId);
        Assert.Equal(custId, criteria.AgentId);
        Assert.Equal(matchId, criteria.MatchId);
        Assert.Equal(betType, criteria.BetType);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectKey()
    {
        // Arrange
        var criteria = new GetOUCriteria(1, 1, 1, 1);

        // Act
        string result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Forecast_SelOU", result);
    }

    [Fact]
    public void InheritsFromGetHDPCriteria()
    {
        // Arrange & Act
        var criteria = new GetOUCriteria(1, 1, 1, 1);

        // Assert
        Assert.IsAssignableFrom<GetHDPCriteria>(criteria);
    }
}
