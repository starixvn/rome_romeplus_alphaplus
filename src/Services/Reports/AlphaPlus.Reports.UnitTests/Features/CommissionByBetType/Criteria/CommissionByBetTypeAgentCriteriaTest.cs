using AlphaPlus.Reports.Features.CommissionByBetType;

namespace AlphaPlus.Reports.UnitTests.Features.CommissionByBetType;

[Trait("Category", "CommissionByBetType")]
public class CommissionByBetTypeAgentCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var moveBetsDay = new DateTime(2023, 1, 15);
        byte levelType = 2;

        // Act
        var criteria = new CommissionByBetTypeAgentCriteria(
            custId, fromDate, toDate, moveBetsDay, levelType);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(levelType, criteria.LevelType);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new CommissionByBetTypeAgentCriteria(
            123, DateTime.Now, DateTime.Now, DateTime.Now, 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_CommByBettype_Agent", result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(255)]
    public void LevelType_ShouldBeSetCorrectly_ForDifferentValues(byte levelType)
    {
        // Arrange & Act
        var criteria = new CommissionByBetTypeAgentCriteria(
            123, DateTime.Now, DateTime.Now, DateTime.Now, levelType);

        // Assert
        Assert.Equal(levelType, criteria.LevelType);
    }
}