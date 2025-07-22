using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.PendingDeposit.Criteria;

public class GetAccumulateCreditCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var criteria = new GetAccumulateCreditCriteria();

        // Assert
        Assert.Equal(0, criteria.CustId);
        Assert.Equal(0, criteria.AccumCreditReturn);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var criteria = new GetAccumulateCreditCriteria();
        var custId = 123;
        var accumCreditReturn = 500.75m;

        // Act
        criteria.CustId = custId;
        criteria.AccumCreditReturn = accumCreditReturn;

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(accumCreditReturn, criteria.AccumCreditReturn);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetAccumulateCreditCriteria();
        var expectedKey = "Age_CashAccumCredit_AgSel";

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldValidateCustId(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetAccumulateCreditCriteria { CustId = custId };

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }
}
