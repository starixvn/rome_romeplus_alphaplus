using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "Promotion")]
public class GetManualBonusCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);

        // Act
        var criteria = new GetManualBonusCriteria(custId, fromDate, toDate);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetManualBonusCriteria(1, DateTime.Now, DateTime.Now);
        var expectedKey = "Age_CashPromotion_ManualBonus_Sel";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_CustId_Is_Greater_Than_Zero()
    {
        // Arrange
        var criteria = new GetManualBonusCriteria(1, DateTime.Now, DateTime.Now);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_CustId_Is_Zero()
    {
        // Arrange
        var criteria = new GetManualBonusCriteria(0, DateTime.Now, DateTime.Now);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_CustId_Is_Negative()
    {
        // Arrange
        var criteria = new GetManualBonusCriteria(-1, DateTime.Now, DateTime.Now);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}