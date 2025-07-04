using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Transactions.Criteria;

public class RevertWithdrawalTransactionToPendingCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new RevertWithdrawalTransactionToPendingCommand();

        // Assert
        Assert.Equal(0, command.TransId);
        Assert.Null(command.Remark);
        Assert.Equal(0, command.UserId);
        Assert.Null(command.Ip);
        Assert.Equal(0, command.SiteDbGroupId);
        Assert.Equal(0, command.ErrCode);
        Assert.Null(command.ErrMsg);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var command = new RevertWithdrawalTransactionToPendingCommand();
        var transId = 98765L;
        var remark = "Reverting withdrawal";
        var userId = 10;
        var ip = "192.168.1.1";
        var siteDbGroupId = 3;
        var errCode = 0;
        var errMsg = "Success";

        // Act
        command.TransId = transId;
        command.Remark = remark;
        command.UserId = userId;
        command.Ip = ip;
        command.SiteDbGroupId = siteDbGroupId;
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;

        // Assert
        Assert.Equal(transId, command.TransId);
        Assert.Equal(remark, command.Remark);
        Assert.Equal(userId, command.UserId);
        Assert.Equal(ip, command.Ip);
        Assert.Equal(siteDbGroupId, command.SiteDbGroupId);
        Assert.Equal(errCode, command.ErrCode);
        Assert.Equal(errMsg, command.ErrMsg);
    }

    [Theory]
    [InlineData(1, "SiteDb_1_Age_CashTransaction_RevertWithdrawal_Upd")]
    [InlineData(7, "SiteDb_7_Age_CashTransaction_RevertWithdrawal_Upd")]
    public void GetSettingKey_ShouldReturnCorrectFormat(int siteDbGroupId, string expectedKey)
    {
        // Arrange
        var command = new RevertWithdrawalTransactionToPendingCommand { SiteDbGroupId = siteDbGroupId };

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, settingKey);
    }

    [Theory]
    [InlineData(123, true)]
    [InlineData(0, false)]
    [InlineData(-5, false)]
    public void IsValid_ShouldValidateTransId(long transId, bool expected)
    {
        // Arrange
        var command = new RevertWithdrawalTransactionToPendingCommand { TransId = transId };

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }
}
