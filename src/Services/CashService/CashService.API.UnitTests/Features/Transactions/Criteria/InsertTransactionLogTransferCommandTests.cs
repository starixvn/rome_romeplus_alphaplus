using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using Xunit;

namespace CashService.API.UnitTests.Features.Transactions.Criteria;

public class InsertTransactionLogTransferCommandTests
{
    private TransactionLog CreateValidLogItem() => new TransactionLog
    {
        UserId = 1,
        FromCustId = 2,
        ToCustId = 3,
        IP = "127.0.0.1",
        Doing = "Test Transfer"
    };

    [Fact]
    public void Constructor_WithValidLogItem_ShouldInitializePropertiesFromLogItem()
    {
        // Arrange
        var logItem = CreateValidLogItem();
        var siteDbGroupId = 1;

        // Act
        var command = new InsertTransactionLogTransferCommand(logItem, siteDbGroupId);

        // Assert
        Assert.Equal(logItem.UserId, command.UserId);
        Assert.Equal(logItem.FromCustId, command.FromCustId);
        Assert.Equal(logItem.ToCustId, command.ToCustId);
        Assert.Equal(logItem.IP, command.IP);
        Assert.Equal(logItem.Doing, command.Doing);
    }

    [Fact]
    public void Constructor_WithNullLogItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        TransactionLog logItem = null;
        var siteDbGroupId = 1;

        // Act & Assert
        Assert.Throws<System.NullReferenceException>(() => new InsertTransactionLogTransferCommand(logItem, siteDbGroupId));
    }

    [Theory]
    [InlineData(1, "SiteDb_1_WriteLog_CashService_UserLog_Transfer")]
    [InlineData(-1, "WriteLog_CashService_UserLog_Transfer")] // Test case where SiteDbGroupId is not > 0
    [InlineData(0, "SiteDb_0_WriteLog_CashService_UserLog_Transfer")] // Test case where SiteDbGroupId is not > 0
    [InlineData(5, "SiteDb_5_WriteLog_CashService_UserLog_Transfer")]
    public void GetSettingKey_ShouldReturnCorrectFormat(int siteDbGroupId, string expectedKey)
    {
        // Arrange
        var logItem = CreateValidLogItem();
        var command = new InsertTransactionLogTransferCommand(logItem, siteDbGroupId);

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, settingKey);
    }

    [Theory]
    [InlineData(1, 2, 3, true)]  // All valid
    [InlineData(0, 2, 3, false)] // Invalid UserId
    [InlineData(1, 0, 3, false)] // Invalid FromCustId
    [InlineData(1, 2, 0, false)] // Invalid ToCustId
    [InlineData(-1, 2, 3, false)]// Invalid UserId
    public void IsValid_ShouldValidateUserAndCustIds(int userId, int fromCustId, int toCustId, bool expected)
    {
        // Arrange
        var logItem = new TransactionLog { UserId = userId, FromCustId = fromCustId, ToCustId = toCustId };
        var command = new InsertTransactionLogTransferCommand(logItem, 1);

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }

    [Fact]
    public void Properties_CanBeSetAfterConstruction()
    {
        // Arrange
        var initialLogItem = CreateValidLogItem();
        var command = new InsertTransactionLogTransferCommand(initialLogItem, 1);

        var newUserId = 10;
        var newFromCustId = 20;
        var newToCustId = 30;
        var newIP = "192.168.0.1";
        var newDoing = "Updated Transfer";
        var newSiteDbGroupId = 2;

        // Act
        command.UserId = newUserId;
        command.FromCustId = newFromCustId;
        command.ToCustId = newToCustId;
        command.IP = newIP;
        command.Doing = newDoing;
        command.SiteDbGroupId = newSiteDbGroupId;

        // Assert
        Assert.Equal(newUserId, command.UserId);
        Assert.Equal(newFromCustId, command.FromCustId);
        Assert.Equal(newToCustId, command.ToCustId);
        Assert.Equal(newIP, command.IP);
        Assert.Equal(newDoing, command.Doing);
        Assert.Equal(newSiteDbGroupId, command.SiteDbGroupId);
    }
}