using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.PendingWithdrawal.Criteria;

public class GetCashBalanceCriteriaTests
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Act
        var criteria = new GetCashBalanceCriteria();

        // Assert
        Assert.Equal(0, criteria.CustId);
        Assert.Equal(0, criteria.TotalCash);
        Assert.Equal(0, criteria.SiteDbGroupId);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var criteria = new GetCashBalanceCriteria();
        var custId = 1;
        var totalCash = 1500.50m;
        var siteDbGroupId = 2;

        // Act
        criteria.CustId = custId;
        criteria.TotalCash = totalCash;
        criteria.SiteDbGroupId = siteDbGroupId;

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(totalCash, criteria.TotalCash);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
    }

    [Theory]
    [InlineData(1, "SiteDb_1_Age_CashService_CashBalance_Sel")]
    [InlineData(5, "SiteDb_5_Age_CashService_CashBalance_Sel")]
    [InlineData(10, "SiteDb_10_Age_CashService_CashBalance_Sel")]
    public void GetSettingKey_ShouldReturnCorrectKeyWithSiteDbGroupId(int siteDbGroupId, string expectedKey)
    {
        // Arrange
        var criteria = new GetCashBalanceCriteria
        {
            SiteDbGroupId = siteDbGroupId
        };

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldValidateCustIdCorrectly(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetCashBalanceCriteria
        {
            CustId = custId
        };

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void IsValid_WhenOtherPropertiesAreZero_ShouldStillReturnTrueIfCustIdIsValid()
    {
        // Arrange
        var criteria = new GetCashBalanceCriteria
        {
            CustId = 1,
            TotalCash = 0,
            SiteDbGroupId = 0
        };

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void TotalCash_WhenSetToDecimalValue_ShouldStoreCorrectly()
    {
        // Arrange
        var criteria = new GetCashBalanceCriteria();
        var expectedTotalCash = 9999.99m;

        // Act
        criteria.TotalCash = expectedTotalCash;

        // Assert
        Assert.Equal(expectedTotalCash, criteria.TotalCash);
    }

    [Fact]
    public void TotalCash_WhenSetToNegativeValue_ShouldStoreCorrectly()
    {
        // Arrange
        var criteria = new GetCashBalanceCriteria();
        var negativeTotalCash = -100.50m;

        // Act
        criteria.TotalCash = negativeTotalCash;

        // Assert
        Assert.Equal(negativeTotalCash, criteria.TotalCash);
    }
}
