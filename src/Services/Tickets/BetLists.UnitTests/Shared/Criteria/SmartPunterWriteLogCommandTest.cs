using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class SmartPunterWriteLogCommandTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        string listUserName = "testUser";
        int actionId = 123;
        bool isRestricted = true;
        int userId = 456;
        string ip = "192.168.1.1";

        // Act
        var command = new SmartPunterWriteLogCommand(
            listUserName,
            actionId,
            isRestricted,
            userId,
            ip);

        // Assert
        Assert.Equal(listUserName, command.ListUserName);
        Assert.Equal(actionId, command.ActionId);
        Assert.Equal(isRestricted, command.IsRestricted);
        Assert.Equal(userId, command.UserId);
        Assert.Equal(ip, command.IP);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var command = new SmartPunterWriteLogCommand(
            "testUser", 1, false, 2, "192.168.1.1");

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal("Age_UserLog_SmartPunterInsMultiple", settingKey);
    }

    [Theory]
    [InlineData("username", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("   ", true)]
    public void IsValid_ShouldReturnCorrectResult(string listUserName, bool expected)
    {
        // Arrange
        var command = new SmartPunterWriteLogCommand(
            listUserName, 1, false, 2, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}