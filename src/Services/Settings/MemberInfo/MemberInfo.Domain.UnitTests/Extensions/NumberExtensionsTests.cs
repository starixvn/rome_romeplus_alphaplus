using AlphaPlus.Settings.MemberInfo.Domain.Extensions;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Extensions;

public class NumberExtensionsTests
{
    [Fact]
    public void ToMoneyFormat_With_Valid_Decimal_Should_Return_Formatted_String()
    {
        // Arrange
        var number = 1234.56m;

        // Act
        var result = number.ToMoneyFormat();

        // Assert
        Assert.Equal("1,234.56", result);
    }

    [Fact]
    public void ToMoneyFormat_With_Zero_Should_Return_Formatted_String()
    {
        // Arrange
        var number = 0m;

        // Act
        var result = number.ToMoneyFormat();

        // Assert
        Assert.Equal("0", result);
    }

    [Fact]
    public void ToMoneyFormat_With_Large_Number_Should_Return_Formatted_String()
    {
        // Arrange
        var number = 1234567.89m;

        // Act
        var result = number.ToMoneyFormat();

        // Assert
        Assert.Equal("1,234,567.89", result);
    }

    [Fact]
    public void ToMoneyFormatWithZeroTail_With_Valid_Decimal_Should_Return_Formatted_String()
    {
        // Arrange
        var number = 1234.5m;

        // Act
        var result = number.ToMoneyFormatWithZeroTail();

        // Assert
        Assert.Equal("1,234.50", result);
    }

    [Fact]
    public void ToMoneyFormatWithZeroTail_With_Zero_Should_Return_Formatted_String()
    {
        // Arrange
        var number = 0m;

        // Act
        var result = number.ToMoneyFormatWithZeroTail();

        // Assert
        Assert.Equal("0.00", result);
    }

    [Fact]
    public void ToMoneyFormatWithZeroTail_With_Large_Number_Should_Return_Formatted_String()
    {
        // Arrange
        var number = 1234567.8m;

        // Act
        var result = number.ToMoneyFormatWithZeroTail();

        // Assert
        Assert.Equal("1,234,567.80", result);
    }

    [Fact]
    public void ToMoneyFormatWithUnlimited_With_Valid_Decimal_Should_Return_Formatted_String()
    {
        // Arrange
        decimal? number = 1234.56m;

        // Act
        var result = number.ToMoneyFormatWithUnlimited();

        // Assert
        Assert.Equal("1,234.56", result);
    }

    [Fact]
    public void ToMoneyFormatWithUnlimited_With_Null_Should_Return_Unlimited()
    {
        // Arrange
        decimal? number = null;

        // Act
        var result = number.ToMoneyFormatWithUnlimited();

        // Assert
        Assert.Equal(Resources.Unlimited, result);
    }

    [Fact]
    public void ToMoneyFormatWithUnlimited_With_Zero_Should_Return_Formatted_String()
    {
        // Arrange
        decimal? number = 0m;

        // Act
        var result = number.ToMoneyFormatWithUnlimited();

        // Assert
        Assert.Equal("0", result);
    }
}
