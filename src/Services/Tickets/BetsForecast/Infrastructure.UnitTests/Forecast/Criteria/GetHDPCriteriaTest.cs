using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetHDPCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        byte custLevel = 1;
        int custId = 123;
        int matchId = 456;
        int betType = 789;

        // Act
        var criteria = new GetHDPCriteria(custLevel, custId, matchId, betType);

        // Assert
        Assert.Equal(custLevel, criteria.RoleId);
        Assert.Equal(custId, criteria.AgentId);
        Assert.Equal(matchId, criteria.MatchId);
        Assert.Equal(betType, criteria.BetType);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Value()
    {
        // Arrange
        var criteria = new GetHDPCriteria(1, 123, 456, 789);

        // Act
        string result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Forecast_SelHDP", result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_All_Values_Are_Valid()
    {
        // Arrange
        var criteria = new GetHDPCriteria(1, 123, 456, 789);

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 123, 456, 789)]
    [InlineData(1, 0, 456, 789)]
    [InlineData(1, 123, 0, 789)]
    [InlineData(1, 123, 456, 0)]
    public void IsValid_Should_Return_False_When_Any_Value_Is_Invalid(byte roleId, int agentId, int matchId, int betType)
    {
        // Arrange
        var criteria = new GetHDPCriteria(roleId, agentId, matchId, betType);

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}
