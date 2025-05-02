using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class SmartPunterSMACheckerCommandTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int custId = 12345;
        string username = "testUser";

        // Act
        var command = new SmartPunterSMACheckerCommand(custId, username);

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.Equal(username, command.Username);
        Assert.False(command.IsSmartPunter);
        Assert.False(command.HasDownlineSmartPunter);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var command = new SmartPunterSMACheckerCommand(12345, "testUser");

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal("Age_CustomerSelCheckSmartPunterSMA", settingKey);
    }

    [Theory]
    [InlineData(12345, "username", true)] // Both CustId and Username provided
    [InlineData(12345, null, true)]      // Only CustId provided
    [InlineData(null, "username", true)] // Only Username provided
    [InlineData(null, null, false)]      // Neither provided
    public void IsValid_ShouldReturnCorrectResult(int? custId, string username, bool expected)
    {
        // Arrange
        var command = new SmartPunterSMACheckerCommand(custId, username);

        // Act
        var result = command.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}