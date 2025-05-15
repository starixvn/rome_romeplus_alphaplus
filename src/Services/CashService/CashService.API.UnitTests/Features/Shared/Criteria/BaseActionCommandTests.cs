using Xunit;
using AlphaPlus.CashService.API.Features.Shared.Criteria;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Criteria;

public class BaseActionCommandTests
{
    private class TestBaseActionCommand : BaseActionCommand
    {
        public TestBaseActionCommand(int custId, int subAccId, string ip, string language) 
            : base(custId, subAccId, ip, language)
        {
        }
    }

    [Fact]
    public void IsValid_WhenAllPropertiesAreValid_ReturnsTrue()
    {
        // Arrange
        var command = new TestBaseActionCommand(
            custId: 1,
            subAccId: 1,
            ip: "127.0.0.1",
            language: "en"
        );

        // Act
        var result = command.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 1, "127.0.0.1", "en")] // Invalid CustId
    [InlineData(1, -1, "127.0.0.1", "en")] // Invalid SubAccId
    [InlineData(1, 1, "", "en")] // Invalid IP
    [InlineData(1, 1, null, "en")] // Invalid IP
    [InlineData(1, 1, "127.0.0.1", "")] // Invalid Language
    [InlineData(1, 1, "127.0.0.1", null)] // Invalid Language
    public void IsValid_WhenPropertiesAreInvalid_ReturnsFalse(int custId, int subAccId, string ip, string language)
    {
        // Arrange
        var command = new TestBaseActionCommand(custId, subAccId, ip, language);

        // Act
        var result = command.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetSettingKey_WhenCalled_ReturnsEmptyString()
    {
        // Arrange
        var command = new TestBaseActionCommand(1, 1, "127.0.0.1", "en");

        // Act
        var result = command.GetSettingKey();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Constructor_WhenCalled_SetsPropertiesCorrectly()
    {
        // Arrange
        var custId = 1;
        var subAccId = 2;
        var ip = "127.0.0.1";
        var language = "en";

        // Act
        var command = new TestBaseActionCommand(custId, subAccId, ip, language);

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.Equal(subAccId, command.SubAccId);
        Assert.Equal(ip, command.IP);
        Assert.Equal(language, command.Language);
    }
} 