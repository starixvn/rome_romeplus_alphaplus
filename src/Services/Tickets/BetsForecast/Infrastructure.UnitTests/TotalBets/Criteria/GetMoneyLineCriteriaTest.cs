using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetMoneyLineCriteriaTest
{
    [Theory]
    [InlineData(4, "Age_TotalBets_Sel2021_SAg")] // Super level
    [InlineData(3, "Age_TotalBets_Sel2021_MAg")]  // Master level
    [InlineData(2, "Age_TotalBets_Sel2021_Ag")]    // Regular level
    public void GetSettingKey_Should_Return_Correct_Key_Based_On_CustLevel(byte custLevel, string expectedKey)
    {
        // Arrange
        var criteria = new GetMoneyLineCriteria(custLevel, custId: 123);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }
}