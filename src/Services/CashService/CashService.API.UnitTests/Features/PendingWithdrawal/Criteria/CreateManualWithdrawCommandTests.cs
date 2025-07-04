using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.PendingWithdrawal.Criteria;

public class CreateManualWithdrawCommandTests
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Act
        var command = new CreateManualWithdrawCommand();

        // Assert
        Assert.Equal(0, command.CustId);
        Assert.Equal(0, command.SubAccId);
        Assert.Equal(0, command.UplineId);
        Assert.Null(command.MinLimitAmt);
        Assert.Null(command.MaxLimitAmt);
        Assert.Null(command.PaymentFrom);
        Assert.Null(command.PaymentTo);
        Assert.Equal(0, command.Amount);
        Assert.Null(command.PaySlip);
        Assert.Null(command.Ip);
        Assert.Null(command.Remark);
        Assert.Equal(0, command.ProviderId);
        Assert.Equal(0, command.PaymentMethodId);
        Assert.Equal(0, command.SiteDbGroupId);
        Assert.Equal(0, command.TransId);
        Assert.Null(command.CashAccumCreditTbl);
        Assert.Equal(0, command.ErrCode);
        Assert.Null(command.ErrMsg);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var command = new CreateManualWithdrawCommand();
        var custId = 1;
        var subAccId = 2;
        var uplineId = 3;
        var minLimitAmt = 100m;
        var maxLimitAmt = 1000m;
        var paymentFrom = "Bank A";
        var paymentTo = "Bank B";
        var amount = 500m;
        var paySlip = new byte[] { 1, 2, 3 };
        var ip = "127.0.0.1";
        var remark = "Test withdrawal";
        var providerId = 4;
        var paymentMethodId = 5;
        var siteDbGroupId = 6;
        var transId = 123456L;
        var cashAccumCreditTbl = "TestTable";
        var errCode = 0;
        var errMsg = "Success";

        // Act
        command.CustId = custId;
        command.SubAccId = subAccId;
        command.UplineId = uplineId;
        command.MinLimitAmt = minLimitAmt;
        command.MaxLimitAmt = maxLimitAmt;
        command.PaymentFrom = paymentFrom;
        command.PaymentTo = paymentTo;
        command.Amount = amount;
        command.PaySlip = paySlip;
        command.Ip = ip;
        command.Remark = remark;
        command.ProviderId = providerId;
        command.PaymentMethodId = paymentMethodId;
        command.SiteDbGroupId = siteDbGroupId;
        command.TransId = transId;
        command.CashAccumCreditTbl = cashAccumCreditTbl;
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.Equal(subAccId, command.SubAccId);
        Assert.Equal(uplineId, command.UplineId);
        Assert.Equal(minLimitAmt, command.MinLimitAmt);
        Assert.Equal(maxLimitAmt, command.MaxLimitAmt);
        Assert.Equal(paymentFrom, command.PaymentFrom);
        Assert.Equal(paymentTo, command.PaymentTo);
        Assert.Equal(amount, command.Amount);
        Assert.Equal(paySlip, command.PaySlip);
        Assert.Equal(ip, command.Ip);
        Assert.Equal(remark, command.Remark);
        Assert.Equal(providerId, command.ProviderId);
        Assert.Equal(paymentMethodId, command.PaymentMethodId);
        Assert.Equal(siteDbGroupId, command.SiteDbGroupId);
        Assert.Equal(transId, command.TransId);
        Assert.Equal(cashAccumCreditTbl, command.CashAccumCreditTbl);
        Assert.Equal(errCode, command.ErrCode);
        Assert.Equal(errMsg, command.ErrMsg);
    }

    [Theory]
    [InlineData(1, "SiteDb_1_Age_CashWithdrawal_ManualAdd")]
    [InlineData(5, "SiteDb_5_Age_CashWithdrawal_ManualAdd")]
    [InlineData(10, "SiteDb_10_Age_CashWithdrawal_ManualAdd")]
    public void GetSettingKey_ShouldReturnCorrectKeyWithSiteDbGroupId(int siteDbGroupId, string expectedKey)
    {
        // Arrange
        var command = new CreateManualWithdrawCommand
        {
            SiteDbGroupId = siteDbGroupId
        };

        // Act
        var settingKey = command.GetSettingKey();

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
        var command = new CreateManualWithdrawCommand
        {
            CustId = custId
        };

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void IsValid_WhenOtherPropertiesAreZeroOrNull_ShouldStillReturnTrueIfCustIdIsValid()
    {
        // Arrange
        var command = new CreateManualWithdrawCommand
        {
            CustId = 1,
            SubAccId = 0,
            UplineId = 0,
            Amount = 0,
            PaymentFrom = null,
            PaymentTo = null
        };

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void NullableProperties_WhenSetToNull_ShouldAllowNullValues()
    {
        // Arrange & Act
        var command = new CreateManualWithdrawCommand
        {
            MinLimitAmt = null,
            MaxLimitAmt = null
        };

        // Assert
        Assert.Null(command.MinLimitAmt);
        Assert.Null(command.MaxLimitAmt);
    }

    [Fact]
    public void PaySlip_WhenSetToByteArray_ShouldStoreCorrectly()
    {
        // Arrange
        var command = new CreateManualWithdrawCommand();
        var paySlip = new byte[] { 0x01, 0x02, 0x03, 0xFF };

        // Act
        command.PaySlip = paySlip;

        // Assert
        Assert.Equal(paySlip, command.PaySlip);
        Assert.Equal(4, command.PaySlip.Length);
    }
}
