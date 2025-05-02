using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetTotalGoalCriteriaTest
{
    [Theory]
    [InlineData(2, 100, "Age_TotalBets_Sel6_Ag")]    // Normal agent level
    [InlineData(3, 200, "Age_TotalBets_Sel6_MAg")]   // Master agent level
    [InlineData(4, 300, "Age_TotalBets_Sel6_SAg")]   // Super agent level
    public void GetSettingKey_ShouldReturn_CorrectKeyForAgentLevel(byte custLevel, int custId, string expectedKey)
    {
        // Arrange
        var criteria = new GetTotalGoalCriteria(custLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties_Correctly()
    {
        // Arrange
        byte custLevel = 2;
        int custId = 123;

        // Act
        var criteria = new GetTotalGoalCriteria(custLevel, custId);

        // Assert
        Assert.Equal(custLevel, criteria.CustLevel.Value);
        Assert.Equal(custId, criteria.CustId);
    }
}
