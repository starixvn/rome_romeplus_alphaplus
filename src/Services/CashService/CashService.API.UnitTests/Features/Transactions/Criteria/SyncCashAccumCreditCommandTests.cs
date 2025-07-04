using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Transactions.Criteria;

public class SyncCashAccumCreditCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new SyncCashAccumCreditCommand();

        // Assert
        Assert.Null(command.CashAccumCreditTbl);
        Assert.Equal(0, command.CustDbGroupId);
        Assert.Equal(0, command.ErrCode);
        Assert.Null(command.ErrMsg);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var command = new SyncCashAccumCreditCommand();
        var cashAccumCreditTbl = "tbl_TestAccum";
        var custDbGroupId = 1;
        var errCode = -1;
        var errMsg = "Sync Error";

        // Act
        command.CashAccumCreditTbl = cashAccumCreditTbl;
        command.CustDbGroupId = custDbGroupId;
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;

        // Assert
        Assert.Equal(cashAccumCreditTbl, command.CashAccumCreditTbl);
        Assert.Equal(custDbGroupId, command.CustDbGroupId);
        Assert.Equal(errCode, command.ErrCode);
        Assert.Equal(errMsg, command.ErrMsg);
    }

    [Theory]
    [InlineData(1, "CustDb_1_Age_CashAccumCredit_SyncData")]
    [InlineData(5, "CustDb_5_Age_CashAccumCredit_SyncData")]
    public void GetSettingKey_ShouldReturnCorrectFormat(int custDbGroupId, string expectedKey)
    {
        // Arrange
        var command = new SyncCashAccumCreditCommand { CustDbGroupId = custDbGroupId };

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, settingKey);
    }

    [Theory]
    [InlineData("tbl_ValidName", true)]
    [InlineData(" ", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValid_ShouldValidateCashAccumCreditTbl(string tableName, bool expected)
    {
        // Arrange
        var command = new SyncCashAccumCreditCommand { CashAccumCreditTbl = tableName };

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }
}
