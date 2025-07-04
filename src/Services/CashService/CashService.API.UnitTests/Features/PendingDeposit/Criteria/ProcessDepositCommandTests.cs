using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.PendingDeposit.Criteria;

public class ProcessDepositCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new ProcessDepositCommand();

        // Assert
        Assert.Equal(0, command.TransId);
        Assert.Equal(0, command.NewAmount);
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
        var transId = 1234567890;
        var amount = 150.75m;
        var remark = "Process Test";
        var statusId = 1;
        var errCode = 0;
        var errMsg = "Success";

        // Act
        command.TransId = transId;
        command.NewAmount = amount;
        command.Remark = remark;
        command.StatusId = statusId;
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;

        // Assert
        Assert.Equal(transId, command.TransId);
        Assert.Equal(amount, command.NewAmount);
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
        Assert.Equal($"SiteDb_0_Age_CashDeposit_UpdateTransStatus", settingKey);
    }

    [Theory]
    [InlineData(1000, 1, 2, 3, true)]
    [InlineData(1000, 0, 2, 3, false)]
    [InlineData(0, 1, 2, 3, false)]
    [InlineData(1000, 1, 0, 3, false)]
    [InlineData(1000, 1, 2, 0, false)]
    public void IsValid_WhenCalledReturnExpectedBoolean(long transId, int agentId, int custId, int statusId, bool expectedValue)
    {
        // Arrange
        var command = new ProcessDepositCommand { TransId = transId, AgentId = agentId, CustId = custId, StatusId = statusId };

        // Act & Assert
        Assert.Equal(expectedValue, command.IsValid());
    }
}