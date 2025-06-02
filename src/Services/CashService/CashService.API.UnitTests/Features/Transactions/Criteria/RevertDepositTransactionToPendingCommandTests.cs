using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using Xunit;

namespace CashService.API.UnitTests.Features.Transactions.Criteria;

public class RevertDepositTransactionToPendingCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new RevertDepositTransactionToPendingCommand();

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
        var command = new RevertDepositTransactionToPendingCommand();
        var transId = 12345L;
        var remark = "Test Remark";
        var userId = 1;
        var ip = "127.0.0.1";
        var siteDbGroupId = 2;
        var errCode = -1;
        var errMsg = "Error Occurred";

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
    [InlineData(1, "SiteDb_1_Age_CashTransaction_RevertDeposit_Upd")]
    [InlineData(5, "SiteDb_5_Age_CashTransaction_RevertDeposit_Upd")]
    public void GetSettingKey_ShouldReturnCorrectFormat(int siteDbGroupId, string expectedKey)
    {
        // Arrange
        var command = new RevertDepositTransactionToPendingCommand { SiteDbGroupId = siteDbGroupId };

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldValidateTransId(long transId, bool expected)
    {
        // Arrange
        var command = new RevertDepositTransactionToPendingCommand { TransId = transId };

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }
}
