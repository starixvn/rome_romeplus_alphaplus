using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Criteria;

public class BaseDBCommandTests
{
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldInitializeProperties()
    {
        // Arrange
        int custId = 1;
        int subAccId = 2;
        string ip = "192.168.1.1";
        string language = "en";

        // Act
        var command = new BaseDBCommand(custId, subAccId, ip, language);

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.Equal(subAccId, command.SubAccId);
        Assert.Equal(ip, command.IP);
        Assert.Equal(language, command.Language);
    }

    [Fact]
    public void IsValid_WhenAllParametersAreValid_ShouldReturnTrue()
    {
        // Arrange
        var command = new BaseDBCommand(1, 0, "192.168.1.1", "en");

        // Act
        var result = command.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 0, "192.168.1.1", "en")] // Invalid CustId
    [InlineData(1, -1, "192.168.1.1", "en")] // Invalid SubAccId
    [InlineData(1, 0, "", "en")] // Invalid IP
    [InlineData(1, 0, null, "en")] // Invalid IP
    [InlineData(1, 0, "192.168.1.1", "")] // Invalid Language
    [InlineData(1, 0, "192.168.1.1", null)] // Invalid Language
    public void IsValid_WhenAnyParameterIsInvalid_ShouldReturnFalse(int custId, int subAccId, string ip, string language)
    {
        // Arrange
        var command = new BaseDBCommand(custId, subAccId, ip, language);

        // Act
        var result = command.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetSettingKey_WhenCalled_ShouldReturnEmptyString()
    {
        // Arrange
        var command = new BaseDBCommand(1, 0, "192.168.1.1", "en");

        // Act
        var result = command.GetSettingKey();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ErrorProperties_WhenSet_ShouldStoreValues()
    {
        // Arrange
        var command = new BaseDBCommand(1, 0, "192.168.1.1", "en");
        int errCode = 500;
        string errMsg = "Test error message";

        // Act
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;

        // Assert
        Assert.Equal(errCode, command.ErrCode);
        Assert.Equal(errMsg, command.ErrMsg);
    }
} 