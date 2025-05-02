using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class SmartPunterWriteLogNoDataCommandTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int? custId = 12345;
        int actionId = 123;
        bool isRestricted = true;
        int userId = 456;
        string ip = "192.168.1.1";

        // Act
        var command = new SmartPunterWriteLogNoDataCommand(
            custId,
            actionId,
            isRestricted,
            userId,
            ip);

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.Equal(actionId, command.ActionId);
        Assert.Equal(isRestricted, command.IsRestricted);
        Assert.Equal(userId, command.UserId);
        Assert.Equal(ip, command.IP);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var command = new SmartPunterWriteLogNoDataCommand(
            12345, 1, false, 2, "192.168.1.1");

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal("NAP_UserLog_SmartPunterIns", settingKey);
    }

    [Theory]
    [InlineData(12345, true)]  // Valid CustId
    [InlineData(1, true)]      // Valid CustId (minimum value)
    [InlineData(0, false)]     // Invalid CustId (zero)
    [InlineData(-1, false)]    // Invalid CustId (negative)
    [InlineData(null, true)]   // No CustId (null is valid according to IsValid implementation)
    public void IsValid_ShouldReturnCorrectResult(int? custId, bool expected)
    {
        // Arrange
        var command = new SmartPunterWriteLogNoDataCommand(
            custId, 1, false, 2, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}