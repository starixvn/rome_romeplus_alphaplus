using AlphaPlus.Reports.Features.CommissionByBetType;

namespace AlphaPlus.Reports.UnitTests.Features.CommissionByBetType;

[Trait("Category", "CommissionByBetType")]
public class CommissionByBetTypeMemberCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var moveBetsDay = new DateTime(2023, 1, 15);

        // Act
        var criteria = new CommissionByBetTypeMemberCriteria(
            custId, fromDate, toDate, moveBetsDay);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new CommissionByBetTypeMemberCriteria(
            123, DateTime.Now, DateTime.Now, DateTime.Now);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_CommByBettype_Member", result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCustIdIsPositiveAndFromDateIsEarlierThanOrEqualToToDate()
    {
        // Arrange
        var criteria = new CommissionByBetTypeMemberCriteria(
            123, new DateTime(2023, 1, 1), new DateTime(2023, 1, 31), DateTime.Now);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsNotPositive(int custId)
    {
        // Arrange
        var criteria = new CommissionByBetTypeMemberCriteria(
            custId, new DateTime(2023, 1, 1), new DateTime(2023, 1, 31), DateTime.Now);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenFromDateIsLaterThanToDate()
    {
        // Arrange
        var criteria = new CommissionByBetTypeMemberCriteria(
            123, new DateTime(2023, 1, 31), new DateTime(2023, 1, 1), DateTime.Now);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}