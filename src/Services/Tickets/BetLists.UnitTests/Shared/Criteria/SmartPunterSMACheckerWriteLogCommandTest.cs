using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class SmartPunterSMACheckerWriteLogCommandTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        string listUserName = "testUser";
        int actionId = 1;
        bool isRestricted = true;
        int userId = 12345;
        string ip = "192.168.1.1";

        // Act
        var command = new SmartPunterSMACheckerWriteLogCommand(listUserName, actionId, isRestricted, userId, ip);

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
        var command = new SmartPunterSMACheckerWriteLogCommand("testUser", 1, true, 12345, "192.168.1.1");

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal("Age_CustomerSelCheckListSmartPunter", settingKey);
    }

    [Theory]
    [InlineData("testUser", true)]  // Valid username
    [InlineData("", false)]         // Empty username
    [InlineData(null, false)]       // Null username
    public void IsValid_ShouldReturnCorrectResult(string listUserName, bool expected)
    {
        // Arrange
        var command = new SmartPunterSMACheckerWriteLogCommand(listUserName, 1, true, 12345, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}