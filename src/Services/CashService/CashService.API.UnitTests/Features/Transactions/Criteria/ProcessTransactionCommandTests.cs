using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Transactions.Criteria;

using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;

public class ProcessTransactionCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new ProcessWithdrawalTransactionCommand();

        // Assert
        Assert.Equal(0, command.TransId);
        Assert.Null(command.PaymentFrom);
        Assert.Null(command.Remark);
        Assert.Null(command.Payslip);
        Assert.Equal(0, command.StatusId);
        Assert.Equal(0, command.ErrCode);
        Assert.Null(command.ErrMsg);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var command = new ProcessWithdrawalTransactionCommand();
        var transId = 1234567890123456789;
        var paymentFrom = "Online Banking";
        var remark = "Processing transaction";
        var payslip = new byte[] { 1, 2, 3 };
        var statusId = 1; // Example status
        var errCode = 0;
        var errMsg = "Success";

        // Act
        command.TransId = transId;
        command.PaymentFrom = paymentFrom;
        command.Remark = remark;
        command.Payslip = payslip;
        command.StatusId = statusId;
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;

        // Assert
        Assert.Equal(transId, command.TransId);
        Assert.Equal(paymentFrom, command.PaymentFrom);
        Assert.Equal(remark, command.Remark);
        Assert.Equal(payslip, command.Payslip);
        Assert.Equal(statusId, command.StatusId);
        Assert.Equal(errCode, command.ErrCode);
        Assert.Equal(errMsg, command.ErrMsg);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnEmptyString()
    {
        // Arrange
        var command = new ProcessWithdrawalTransactionCommand();

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal("SiteDb_0_Age_CashWithdrawal_UpdateTransStatus", settingKey);
    }

    [Theory]
    [InlineData(1000, 1, 1, "abc", true)]
    [InlineData(0, 1, 1, "abc", false)]
    [InlineData(1000, 0, 1, "abc", false)]
    [InlineData(1000, 1, 0, "abc", false)]
    [InlineData(1000, 1, 1, "", true)]
    public void IsValid_Returns_ExpectedResult(long transId, int agentId, int custId, string paymentFrom, bool result)
    {
        // Arrange
        var command = new ProcessWithdrawalTransactionCommand { TransId = transId, AgentId = agentId, CustId = custId, PaymentFrom = paymentFrom };

        // Act & Assert
        Assert.Equal(result, command.IsValid());
    }

    [Fact]
    public void Payslip_WhenSetToNull_ShouldBeNull()
    {
        // Arrange
        var command = new ProcessWithdrawalTransactionCommand
        {
            // Act
            Payslip = null
        };

        // Assert
        Assert.Null(command.Payslip);
    }
}