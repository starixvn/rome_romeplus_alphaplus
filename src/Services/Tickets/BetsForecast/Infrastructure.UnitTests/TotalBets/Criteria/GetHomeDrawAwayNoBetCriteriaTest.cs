using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetHomeDrawAwayNoBetCriteriaTest
{
    [Theory]
    [InlineData(2, 100, "Age_TotalBets_Sel121_Ag")]     // Regular agent level
    [InlineData(3, 200, "Age_TotalBets_Sel121_MAg")]    // Master level
    [InlineData(4, 300, "Age_TotalBets_Sel121_SAg")]    // Super level
    public void GetSettingKey_WithDifferentCustLevels_ShouldReturnCorrectKey(byte custLevel, int custId, string expectedKey)
    {
        // Arrange
        var criteria = new GetHomeDrawAwayNoBetCriteria(custLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }
}
