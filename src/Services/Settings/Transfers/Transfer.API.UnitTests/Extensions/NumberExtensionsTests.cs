using AlphaPlus.Settings.Transfer.API.Extensions;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Extensions;

public class NumberExtensionsTests
{
    [Fact]
    public void FormatWithCommasAlwaysWithDecimalPoint_Should_Format_Integer_Correctly()
    {
        // Arrange
        decimal number = 1234;

        // Act
        var result = number.FormatWithCommasAlwaysWithDecimalPoint();

        // Assert
        Assert.Equal("1,234.00", result);
    }

    [Fact]
    public void FormatWithCommasAlwaysWithDecimalPoint_Should_Format_Decimal_With_Two_Decimal_Places()
    {
        // Arrange
        decimal number = 1234.567m;

        // Act
        var result = number.FormatWithCommasAlwaysWithDecimalPoint();

        // Assert
        Assert.Equal("1,234.57", result);
    }

    [Fact]
    public void FormatWithCommasAlwaysWithDecimalPoint_Should_Handle_Large_Numbers()
    {
        // Arrange
        decimal number = 1234567.89m;

        // Act
        var result = number.FormatWithCommasAlwaysWithDecimalPoint();

        // Assert
        Assert.Equal("1,234,567.89", result);
    }

    [Fact]
    public void FormatWithCommasAlwaysWithDecimalPoint_Should_Handle_Zero()
    {
        // Arrange
        decimal number = 0;

        // Act
        var result = number.FormatWithCommasAlwaysWithDecimalPoint();

        // Assert
        Assert.Equal("0.00", result);
    }

    [Fact]
    public void FormatWithCommasAlwaysWithDecimalPoint_Should_Handle_Negative_Numbers()
    {
        // Arrange
        decimal number = -1234.56m;

        // Act
        var result = number.FormatWithCommasAlwaysWithDecimalPoint();

        // Assert
        Assert.Equal("-1,234.56", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Format_Integer_Without_Decimals()
    {
        // Arrange
        decimal number = 1234;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("1,234", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Format_With_Decimals_When_Present()
    {
        // Arrange
        decimal number = 1234.50m;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("1,234.5", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Remove_Trailing_Zeros()
    {
        // Arrange
        decimal number = 1234.00m;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("1,234", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Handle_Single_Decimal()
    {
        // Arrange
        decimal number = 1234.5m;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("1,234.5", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Handle_Large_Numbers()
    {
        // Arrange
        decimal number = 1234567.89m;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("1,234,567.89", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Handle_Zero()
    {
        // Arrange
        decimal number = 0;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("0", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Handle_Negative_Numbers()
    {
        // Arrange
        decimal number = -1234.56m;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("-1,234.56", result);
    }

    [Fact]
    public void FormatWithCommasOptionalWithDecimalPoint_Should_Handle_Very_Small_Decimals()
    {
        // Arrange
        decimal number = 0.01m;

        // Act
        var result = number.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal("0.01", result);
    }

    [Theory]
    [InlineData(1000, "1,000.00", "1,000")]
    [InlineData(1000000, "1,000,000.00", "1,000,000")]
    [InlineData(123.456, "123.46", "123.46")]
    [InlineData(123.4, "123.40", "123.4")]
    [InlineData(0.5, "0.50", "0.5")]
    public void NumberExtensions_Should_Format_Correctly(decimal input, string expectedAlways, string expectedOptional)
    {
        // Act
        var resultAlways = input.FormatWithCommasAlwaysWithDecimalPoint();
        var resultOptional = input.FormatWithCommasOptionalWithDecimalPoint();

        // Assert
        Assert.Equal(expectedAlways, resultAlways);
        Assert.Equal(expectedOptional, resultOptional);
    }
}
