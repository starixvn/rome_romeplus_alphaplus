using AlphaPlus.Core.Extensions;

namespace AlphaPlus.Core.UnitTests.Extensions;

public class NumberExtensionsTests
{
    [Theory]
    [InlineData("1234.567", "1,234.57")]
    [InlineData("0.7", "0.7")]
    [InlineData("0", "0")]
    [InlineData("1000000.123", "1,000,000.12")]
    [InlineData("0.123", "0.12")]
    [InlineData("999.999", "1,000")]
    [InlineData("-1234.567", "-1,234.57")]
    [InlineData("0.001", "0")]
    public void FormatNumber2_Should_Format_With_2_Decimals_If_Any(string valueStr, string expected)
    {
        // Arrange
        var value = decimal.Parse(valueStr);

        // Act
        var result = value.FormatNumber2();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1234.567", "1,234.57")]
    [InlineData("0.7", "0.70")]
    [InlineData("0", "0.00")]
    [InlineData("1000000.123", "1,000,000.12")]
    [InlineData("0.123", "0.12")]
    [InlineData("999.999", "1,000.00")]
    [InlineData("-1234.567", "-1,234.57")]
    [InlineData("0.001", "0.00")]
    public void FormatNumber20_Should_Always_Format_With_2_Decimals(string valueStr, string expected)
    {
        // Arrange
        var value = decimal.Parse(valueStr);

        // Act
        var result = value.FormatNumber20();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1234.5678", "1,234.5678")]
    [InlineData("0.7", "0.7")]
    [InlineData("0", "0")]
    [InlineData("1000000.1234", "1,000,000.1234")]
    [InlineData("0.1234", "0.1234")]
    [InlineData("-1234.5678", "-1,234.5678")]
    public void FormatNumber4_Should_Format_With_4_Decimals_If_Any(string valueStr, string expected)
    {
        // Arrange
        var value = decimal.Parse(valueStr);

        // Act
        var result = value.FormatNumber4();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1234.5678", "1,234.5678")]
    [InlineData("0.7", "0.7000")]
    [InlineData("0", "0.0000")]
    [InlineData("1000000.1234", "1,000,000.1234")]
    [InlineData("0.1234", "0.1234")]
    [InlineData("999.9999", "999.9999")]
    [InlineData("-1234.5678", "-1,234.5678")]
    [InlineData("0.0001", "0.0001")]
    public void FormatNumber40_Should_Always_Format_With_4_Decimals(string valueStr, string expected)
    {
        // Arrange
        var value = decimal.Parse(valueStr);

        // Act
        var result = value.FormatNumber40();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatNumber2_Should_Handle_Edge_Cases()
    {
        // Arrange
        var maxValue = 79228162514264337593543950335m; // decimal.MaxValue equivalent
        var minValue = -79228162514264337593543950335m; // decimal.MinValue equivalent

        // Act
        var maxResult = maxValue.FormatNumber2();
        var minResult = minValue.FormatNumber2();

        // Assert
        Assert.NotEmpty(maxResult);
        Assert.NotEmpty(minResult);
        Assert.Contains(",", maxResult); // Should contain thousands separator
        Assert.Contains(",", minResult); // Should contain thousands separator
    }

    [Fact]
    public void FormatNumber20_Should_Handle_Edge_Cases()
    {
        // Arrange
        var maxValue = 79228162514264337593543950335m; // decimal.MaxValue equivalent
        var minValue = -79228162514264337593543950335m; // decimal.MinValue equivalent

        // Act
        var maxResult = maxValue.FormatNumber20();
        var minResult = minValue.FormatNumber20();

        // Assert
        Assert.NotEmpty(maxResult);
        Assert.NotEmpty(minResult);
        Assert.Contains(",", maxResult); // Should contain thousands separator
        Assert.Contains(",", minResult); // Should contain thousands separator
        Assert.EndsWith(".00", maxResult); // Should always end with .00
        Assert.EndsWith(".00", minResult); // Should always end with .00
    }

    [Fact]
    public void FormatNumber4_Should_Handle_Edge_Cases()
    {
        // Arrange
        var maxValue = 79228162514264337593543950335m; // decimal.MaxValue equivalent
        var minValue = -79228162514264337593543950335m; // decimal.MinValue equivalent

        // Act
        var maxResult = maxValue.FormatNumber4();
        var minResult = minValue.FormatNumber4();

        // Assert
        Assert.NotEmpty(maxResult);
        Assert.NotEmpty(minResult);
        Assert.Contains(",", maxResult); // Should contain thousands separator
        Assert.Contains(",", minResult); // Should contain thousands separator
    }

    [Fact]
    public void FormatNumber40_Should_Handle_Edge_Cases()
    {
        // Arrange
        var maxValue = 79228162514264337593543950335m; // decimal.MaxValue equivalent
        var minValue = -79228162514264337593543950335m; // decimal.MinValue equivalent

        // Act
        var maxResult = maxValue.FormatNumber40();
        var minResult = minValue.FormatNumber40();

        // Assert
        Assert.NotEmpty(maxResult);
        Assert.NotEmpty(minResult);
        Assert.Contains(",", maxResult); // Should contain thousands separator
        Assert.Contains(",", minResult); // Should contain thousands separator
        Assert.EndsWith(".0000", maxResult); // Should always end with .0000
        Assert.EndsWith(".0000", minResult); // Should always end with .0000
    }

    [Theory]
    [InlineData("0.0001")]
    [InlineData("0.001")]
    [InlineData("0.01")]
    [InlineData("0.1")]
    [InlineData("1")]
    [InlineData("10")]
    [InlineData("100")]
    [InlineData("1000")]
    [InlineData("10000")]
    public void All_Format_Methods_Should_Handle_Positive_Numbers(string valueStr)
    {
        // Arrange
        var value = decimal.Parse(valueStr);

        // Act
        var result2 = value.FormatNumber2();
        var result20 = value.FormatNumber20();
        var result4 = value.FormatNumber4();
        var result40 = value.FormatNumber40();

        // Assert
        Assert.NotEmpty(result2);
        Assert.NotEmpty(result20);
        Assert.NotEmpty(result4);
        Assert.NotEmpty(result40);
    }
}
