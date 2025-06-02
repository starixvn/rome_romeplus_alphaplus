using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class InternalAdminCheckerCommandTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int custId = 123;

        // Act
        var command = new InternalAdminCheckerCommand(custId);

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.False(command.IsInternalAdmin); // Default value should be false
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectKey()
    {
        // Arrange
        var command = new InternalAdminCheckerCommand(1);

        // Act
        var key = command.GetSettingKey();

        // Assert
        Assert.Equal("Acc_CustomerIsInternalAdmin", key);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ReturnsTrueIfCustIdIsPositive(int custId, bool expected)
    {
        // Arrange
        var command = new InternalAdminCheckerCommand(custId);

        // Act
        var result = command.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}
