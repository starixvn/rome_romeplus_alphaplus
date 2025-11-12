using AlphaPlus.Settings.ViewLogs.API.Extensions;

namespace AlphaPlus.Settings.ViewLogs.API.UnitTests.Extensions;

public class DecimalExtensionTests
{
    [Fact]
    public void ToNumericString_Should_Format_Integer_Correctly()
    {
        // Arrange
        decimal value = 1234;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("1,234", result);
    }

    [Fact]
    public void ToNumericString_Should_Format_Decimal_With_Two_Decimal_Places()
    {
        // Arrange
        decimal value = 1234.567m;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("1,234.57", result);
    }

    [Fact]
    public void ToNumericString_Should_Handle_Large_Numbers()
    {
        // Arrange
        decimal value = 1234567.89m;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("1,234,567.89", result);
    }

    [Fact]
    public void ToNumericString_Should_Handle_Zero()
    {
        // Arrange
        decimal value = 0;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("0", result);
    }

    [Fact]
    public void ToNumericString_Should_Handle_Negative_Numbers()
    {
        // Arrange
        decimal value = -1234.56m;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("-1,234.56", result);
    }

    [Fact]
    public void ToNumericString_Should_Remove_Trailing_Zeros_From_Decimal()
    {
        // Arrange
        decimal value = 1234.00m;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("1,234", result);
    }

    [Fact]
    public void ToNumericString_Should_Handle_Single_Decimal_Place()
    {
        // Arrange
        decimal value = 1234.5m;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("1,234.5", result);
    }

    [Fact]
    public void ToNumericString_Should_Handle_Very_Small_Decimals()
    {
        // Arrange
        decimal value = 0.01m;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("0.01", result);
    }

    [Fact]
    public void ToNumericString_Should_Handle_Very_Large_Numbers()
    {
        // Arrange
        decimal value = 999999999.99m;

        // Act
        var result = value.ToNumericString();

        // Assert
        Assert.Equal("999,999,999.99", result);
    }

    [Theory]
    [InlineData(1000, "1,000")]
    [InlineData(1000000, "1,000,000")]
    [InlineData(123.456, "123.46")]
    [InlineData(123.4, "123.4")]
    [InlineData(0.5, "0.5")]
    public void ToNumericString_Should_Format_Correctly(decimal input, string expected)
    {
        // Act
        var result = input.ToNumericString();

        // Assert
        Assert.Equal(expected, result);
    }
}
