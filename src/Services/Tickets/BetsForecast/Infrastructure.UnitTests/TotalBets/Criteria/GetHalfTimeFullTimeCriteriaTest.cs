using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetHalfTimeFullTimeCriteriaTest
{
    [Theory]
    [InlineData(2, 123, "Age_TotalBets_Sel16_Ag")]    // Regular level
    [InlineData(3, 456, "Age_TotalBets_Sel16_MAg")]   // Master level
    [InlineData(4, 789, "Age_TotalBets_Sel16_SAg")]   // Super level
    public void GetSettingKey_Returns_CorrectKey_BasedOn_CustLevel(byte custLevel, int custId, string expectedKey)
    {
        // Arrange
        var criteria = new GetHalfTimeFullTimeCriteria(custLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void Constructor_Sets_Properties_Correctly()
    {
        // Arrange
        byte custLevel = 2;
        int custId = 456;

        // Act
        var criteria = new GetHalfTimeFullTimeCriteria(custLevel, custId);

        // Assert
        Assert.Equal(custLevel, criteria.CustLevel.Value);
        Assert.Equal(custId, criteria.CustId);
    }
}