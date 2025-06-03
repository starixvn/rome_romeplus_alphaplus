using System.Numerics;
using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using Xunit;

namespace CashService.API.UnitTests.Features.PendingDeposit.Criteria;

public class ProcessDepositCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new ProcessDepositCommand();

        // Assert
        Assert.Equal(BigInteger.Zero, command.TransId);
        Assert.Equal(0, command.Amount);
        Assert.Null(command.Remark);
        Assert.Equal(0, command.StatusId);
        Assert.Equal(0, command.ErrCode);
        Assert.Null(command.ErrMsg);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var command = new ProcessDepositCommand();
        var transId = new BigInteger(1234567890);
        var amount = 150.75m;
        var remark = "Process Test";
        var statusId = 1;
        var errCode = 0;
        var errMsg = "Success";

        // Act
        command.TransId = transId;
        command.Amount = amount;
        command.Remark = remark;
        command.StatusId = statusId;
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;

        // Assert
        Assert.Equal(transId, command.TransId);
        Assert.Equal(amount, command.Amount);
        Assert.Equal(remark, command.Remark);
        Assert.Equal(statusId, command.StatusId);
        Assert.Equal(errCode, command.ErrCode);
        Assert.Equal(errMsg, command.ErrMsg);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnEmptyString()
    {
        // Arrange
        var command = new ProcessDepositCommand();

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal(string.Empty, settingKey);
    }

    [Fact]
    public void IsValid_ShouldThrowNotImplementedException()
    {
        // Arrange
        var command = new ProcessDepositCommand();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => command.IsValid());
    }
}
