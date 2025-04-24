using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.UnitTests.Features.Shared.Extensions;

[Trait("Category", "Shared")]
public class NumberExtensionsTest
{
    [Fact]
    public void FormatNumber_Int_ShouldFormatWithThousandsSeparator()
    {
        // Arrange
        int number = 12345;

        // Act
        var result = number.FormatNumber();

        // Assert
        Assert.Equal("12,345", result);
    }

    [Fact]
    public void FormatNumber_Decimal_ShouldFormatWithTwoDecimalPlaces()
    {
        // Arrange
        decimal number = 12345.6789m;

        // Act
        var result = number.FormatNumber();

        // Assert
        Assert.Equal("12,345.68", result);
    }

    [Fact]
    public void FormatNumber4_Decimal_ShouldFormatWithFourDecimalPlaces()
    {
        // Arrange
        decimal number = 12345.67891m;

        // Act
        var result = number.FormatNumber4();

        // Assert
        Assert.Equal("12,345.6789", result);
    }

    [Fact]
    public void FormatPTPercentage_ShouldMultiplyBy100AndAppendPercentageWithPT()
    {
        // Arrange
        decimal pt = 0.1234m;

        // Act
        var result = pt.FormatPTPercentage();

        // Assert
        Assert.Equal("12.3% " + SharedResources.PT, result);
    }

    [Fact]
    public void FormatNumber_NullableDecimal_ShouldReturnFormattedValueWhenNotNull()
    {
        // Arrange
        decimal? number = 12345.67m;

        // Act
        var result = number.FormatNumber();

        // Assert
        Assert.Equal("12,345.67", result);
    }

    [Fact]
    public void FormatNumber_NullableDecimal_ShouldReturnNotAvailableWhenNull()
    {
        // Arrange
        decimal? number = null;

        // Act
        var result = number.FormatNumber();

        // Assert
        Assert.Equal(NumberExtensions.NotAvailable, result);
    }

    [Fact]
    public void FormatPTPercentage_ShouldRoundToOneDecimalPlace()
    {
        // Arrange
        decimal pt = 0.12345m;

        // Act
        var result = pt.FormatPTPercentage();

        // Assert
        Assert.Equal("12.3% " + SharedResources.PT, result);
    }

    [Fact]
    public void FormatNumber_ZeroInt_ShouldFormatCorrectly()
    {
        // Arrange
        int number = 0;

        // Act
        var result = number.FormatNumber();

        // Assert
        Assert.Equal("0", result);
    }

    [Fact]
    public void FormatNumber_ZeroDecimal_ShouldFormatCorrectly()
    {
        // Arrange
        decimal number = 0m;

        // Act
        var result = number.FormatNumber();

        // Assert
        Assert.Equal("0.00", result);
    }
}