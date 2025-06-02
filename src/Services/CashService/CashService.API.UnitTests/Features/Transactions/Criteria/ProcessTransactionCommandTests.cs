using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using System.Numerics;
using Xunit;

namespace CashService.API.UnitTests.Features.Transactions.Criteria;

public class ProcessTransactionCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new ProcessTransactionCommand();

        // Assert
        Assert.Equal(BigInteger.Zero, command.TransId);
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
        var command = new ProcessTransactionCommand();
        var transId = new BigInteger(1234567890123456789);
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
        var command = new ProcessTransactionCommand();

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal(string.Empty, settingKey);
    }

    [Fact]
    public void IsValid_ShouldThrowNotImplementedException()
    {
        // Arrange
        var command = new ProcessTransactionCommand();

        // Act & Assert
        Assert.Throws<System.NotImplementedException>(() => command.IsValid());
    }

    [Fact]
    public void Payslip_WhenSetToNull_ShouldBeNull()
    {
        // Arrange
        var command = new ProcessTransactionCommand
        {
            // Act
            Payslip = null
        };

        // Assert
        Assert.Null(command.Payslip);
    }
}
