using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetHDPOULiveCriteriaTest
{
    [Theory]
    [InlineData(2, 123, "Age_TotalBets_1378_Member")] // Member level
    [InlineData(3, 123, "Age_TotalBets_1378_Agent")]  // Agent level
    [InlineData(4, 123, "Age_TotalBets_1378_Master")] // Super level
    public void GetSettingKey_Should_Return_Correct_Key_Based_On_CustLevel(byte custLevel, int custId, string expectedKey)
    {
        // Arrange
        var criteria = new GetHDPOULiveCriteria(custLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }
}
