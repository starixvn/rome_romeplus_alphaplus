using AlphaPlus.Reports.Features.CommissionByBetType;

namespace AlphaPlus.Reports.UnitTests.Features.CommissionByBetType;

[Trait("Category", "CommissionByBetType")]
public class CommissionByBetTypeMasterCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        var custId = 456;
        var fromDate = new DateTime(2023, 2, 1);
        var toDate = new DateTime(2023, 2, 28);
        var moveBetsDay = new DateTime(2023, 2, 15);
        byte levelType = 3;

        // Act
        var criteria = new CommissionByBetTypeMasterCriteria(
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
        var criteria = new CommissionByBetTypeMasterCriteria(
            123, DateTime.Now, DateTime.Now, DateTime.Now, 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_CommByBettype_Master", result);
    }

    [Fact]
    public void InheritanceTest_ShouldInheritFromAgentCriteria()
    {
        // Arrange & Act
        var criteria = new CommissionByBetTypeMasterCriteria(
            123, DateTime.Now, DateTime.Now, DateTime.Now, 1);

        // Assert
        Assert.IsAssignableFrom<CommissionByBetTypeAgentCriteria>(criteria);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(255)]
    public void LevelType_ShouldBeSetCorrectly_ForDifferentValues(byte levelType)
    {
        // Arrange & Act
        var criteria = new CommissionByBetTypeMasterCriteria(
            123, DateTime.Now, DateTime.Now, DateTime.Now, levelType);

        // Assert
        Assert.Equal(levelType, criteria.LevelType);
    }
}