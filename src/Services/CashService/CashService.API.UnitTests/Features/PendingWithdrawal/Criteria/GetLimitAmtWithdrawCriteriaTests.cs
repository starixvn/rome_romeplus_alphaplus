using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;
using Xunit;

namespace CashService.API.UnitTests.Features.PendingWithdrawal.Criteria;

public class GetLimitAmtWithdrawCriteriaTests
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Act
        var criteria = new GetLimitAmtWithdrawCriteria();

        // Assert
        Assert.Equal(0, criteria.CustId);
        Assert.Equal(0, criteria.ProviderId);
        Assert.Equal(0, criteria.UplineId);
        Assert.Equal(0, criteria.PaymentMethodId);
        Assert.Equal(0, criteria.CustDbGroupId);
        Assert.Null(criteria.MinAmt);
        Assert.Null(criteria.MaxAmt);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var criteria = new GetLimitAmtWithdrawCriteria();
        var custId = 1;
        var providerId = 2;
        var uplineId = 3;
        var paymentMethodId = 4;
        var custDbGroupId = 5;
        var minAmt = 100m;
        var maxAmt = 1000m;

        // Act
        criteria.CustId = custId;
        criteria.ProviderId = providerId;
        criteria.UplineId = uplineId;
        criteria.PaymentMethodId = paymentMethodId;
        criteria.CustDbGroupId = custDbGroupId;
        criteria.MinAmt = minAmt;
        criteria.MaxAmt = maxAmt;

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(providerId, criteria.ProviderId);
        Assert.Equal(uplineId, criteria.UplineId);
        Assert.Equal(paymentMethodId, criteria.PaymentMethodId);
        Assert.Equal(custDbGroupId, criteria.CustDbGroupId);
        Assert.Equal(minAmt, criteria.MinAmt);
        Assert.Equal(maxAmt, criteria.MaxAmt);
    }

    [Theory]
    [InlineData(1, "CustDb_1_Age_CashWithdrawal_LimitAmtSel")]
    [InlineData(5, "CustDb_5_Age_CashWithdrawal_LimitAmtSel")]
    [InlineData(10, "CustDb_10_Age_CashWithdrawal_LimitAmtSel")]
    public void GetSettingKey_ShouldReturnCorrectKeyWithCustDbGroupId(int custDbGroupId, string expectedKey)
    {
        // Arrange
        var criteria = new GetLimitAmtWithdrawCriteria
        {
            CustDbGroupId = custDbGroupId
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
        var criteria = new GetLimitAmtWithdrawCriteria
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
        var criteria = new GetLimitAmtWithdrawCriteria
        {
            CustId = 1,
            ProviderId = 0,
            UplineId = 0,
            PaymentMethodId = 0
        };

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }
}
