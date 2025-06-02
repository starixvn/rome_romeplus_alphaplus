using AlphaPlus.Reports.Features.IPMatching;

namespace AlphaPlus.Reports.UnitTests.Features.IPMatching;

[Trait("Category", "IPMatching")]
public class GetIPMatchingCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int custId = 123;
        var winlostDate = new DateTime(2023, 1, 10);
        var moveBetsDay = new DateTime(2023, 1, 15);
        int memberCount = 5;

        // Act
        var criteria = new GetIPMatchingCriteria(custId, winlostDate, moveBetsDay, memberCount);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(winlostDate, criteria.WinlostDate);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(memberCount, criteria.MemberCount);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetIPMatchingCriteria(123, DateTime.Now, DateTime.Now, 5);
        var expectedKey = "Age_BetStatistics_IPMatching_Master";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCustIdIsGreaterThanZero()
    {
        // Arrange
        var criteria = new GetIPMatchingCriteria(123, DateTime.Now, DateTime.Now, 5);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsZeroOrLess()
    {
        // Arrange
        var criteriaWithZero = new GetIPMatchingCriteria(0, DateTime.Now, DateTime.Now, 5);
        var criteriaWithNegative = new GetIPMatchingCriteria(-1, DateTime.Now, DateTime.Now, 5);

        // Act
        var resultWithZero = criteriaWithZero.IsValid();
        var resultWithNegative = criteriaWithNegative.IsValid();

        // Assert
        Assert.False(resultWithZero);
        Assert.False(resultWithNegative);
    }
}