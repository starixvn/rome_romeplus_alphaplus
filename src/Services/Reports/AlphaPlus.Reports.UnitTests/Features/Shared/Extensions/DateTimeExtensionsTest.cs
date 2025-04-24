using System;
using AlphaPlus.Reports.Features.Shared.Extensions;
using Xunit;

namespace AlphaPlus.Reports.UnitTests.Features.Shared.Extensions;

[Trait("Category", "Shared")]
public class DateTimeExtensionsTest
{
    private readonly DateTime testDate = new DateTime(2023, 10, 15, 14, 30, 45);

    [Fact]
    public void ToMMDDWithSlash_ShouldReturnCorrectFormat()
    {
        // Arrange & Act
        var result = testDate.ToMMDDWithSlash();

        // Assert
        Assert.Equal("10/15", result);
    }

    [Fact]
    public void ToShortDateWithSlash_ShouldReturnCorrectFormat()
    {
        // Arrange & Act
        var result = testDate.ToShortDateWithSlash();

        // Assert
        Assert.Equal("10/15/2023", result);
    }

    [Fact]
    public void ToLongDateWithSlash_ShouldReturnCorrectFormat()
    {
        // Arrange & Act
        var result = testDate.ToLongDateWithSlash();

        // Assert
        Assert.Equal("10/15/2023 02:30:45 PM", result);
    }

    [Fact]
    public void ToLongDate24HourWithSlash_ShouldReturnCorrectFormat()
    {
        // Arrange & Act
        var result = testDate.ToLongDate24HourWithSlash();

        // Assert
        Assert.Equal("10/15/2023 14:30:45", result);
    }

    [Fact]
    public void ToLongDateWithoutSecondWithSlash_ShouldReturnCorrectFormat()
    {
        // Arrange & Act
        var result = testDate.ToLongDateWithoutSecondWithSlash();

        // Assert
        Assert.Equal("10/15/2023 02:30 PM", result);
    }

    [Fact]
    public void ToMMDDWithSlash_WithDifferentDate_ShouldReturnCorrectFormat()
    {
        // Arrange
        var date = new DateTime(2023, 1, 5, 10, 15, 20);

        // Act
        var result = date.ToMMDDWithSlash();

        // Assert
        Assert.Equal("01/05", result);
    }

    [Fact]
    public void AllFormatters_ShouldHandleYearBoundary()
    {
        // Arrange
        var yearEndDate = new DateTime(2023, 12, 31, 23, 59, 59);

        // Act & Assert
        Assert.Equal("12/31", yearEndDate.ToMMDDWithSlash());
        Assert.Equal("12/31/2023", yearEndDate.ToShortDateWithSlash());
        Assert.Equal("12/31/2023 11:59:59 PM", yearEndDate.ToLongDateWithSlash());
        Assert.Equal("12/31/2023 23:59:59", yearEndDate.ToLongDate24HourWithSlash());
        Assert.Equal("12/31/2023 11:59 PM", yearEndDate.ToLongDateWithoutSecondWithSlash());
    }
}