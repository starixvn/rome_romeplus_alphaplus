using AlphaPlus.IPInfo.Extensions;
using AlphaPlus.IPInfo.Models;

namespace AlphaPlus.IPInfo.UnitTests.Extensions;

public class ListExtensionsTests
{
    [Fact]
    public void BinarySearch2_WithValueInRange_ShouldReturnMatchingRange()
    {
        // Arrange
        var ipRanges = new List<IPRange>
        {
            new(1000, 2000, "Country1"),
            new(3000, 4000, "Country2"),
            new(5000, 6000, "Country3")
        };

        // Act
        var result = ipRanges.BinarySearch2(3500);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Country2", result.Country);
        Assert.Equal(3000, result.Lower);
        Assert.Equal(4000, result.Upper);
    }

    [Fact]
    public void BinarySearch2_WithValueAtLowerBound_ShouldReturnMatchingRange()
    {
        // Arrange
        var ipRanges = new List<IPRange>
        {
            new(1000, 2000, "Country1"),
            new(3000, 4000, "Country2"),
            new(5000, 6000, "Country3")
        };

        // Act
        var result = ipRanges.BinarySearch2(1000);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Country1", result.Country);
        Assert.Equal(1000, result.Lower);
        Assert.Equal(2000, result.Upper);
    }

    [Fact]
    public void BinarySearch2_WithValueAtUpperBound_ShouldReturnMatchingRange()
    {
        // Arrange
        var ipRanges = new List<IPRange>
        {
            new(1000, 2000, "Country1"),
            new(3000, 4000, "Country2"),
            new(5000, 6000, "Country3")
        };

        // Act
        var result = ipRanges.BinarySearch2(2000);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Country1", result.Country);
        Assert.Equal(1000, result.Lower);
        Assert.Equal(2000, result.Upper);
    }

    [Fact]
    public void BinarySearch2_WithValueBelowAllRanges_ShouldReturnNull()
    {
        // Arrange
        var ipRanges = new List<IPRange>
        {
            new(1000, 2000, "Country1"),
            new(3000, 4000, "Country2"),
            new(5000, 6000, "Country3")
        };

        // Act
        var result = ipRanges.BinarySearch2(500);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void BinarySearch2_WithValueAboveAllRanges_ShouldReturnNull()
    {
        // Arrange
        var ipRanges = new List<IPRange>
        {
            new(1000, 2000, "Country1"),
            new(3000, 4000, "Country2"),
            new(5000, 6000, "Country3")
        };

        // Act
        var result = ipRanges.BinarySearch2(7000);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void BinarySearch2_WithValueBetweenRanges_ShouldReturnNull()
    {
        // Arrange
        var ipRanges = new List<IPRange>
        {
            new(1000, 2000, "Country1"),
            new(3000, 4000, "Country2"),
            new(5000, 6000, "Country3")
        };

        // Act
        var result = ipRanges.BinarySearch2(2500);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void BinarySearch2_WithEmptyList_ShouldReturnNull()
    {
        // Arrange
        var ipRanges = new List<IPRange>();

        // Act
        var result = ipRanges.BinarySearch2(1000);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void BinarySearch2_WithSingleRange_ShouldReturnMatchingRange()
    {
        // Arrange
        var ipRanges = new List<IPRange>
        {
            new(1000, 2000, "Country1")
        };

        // Act
        var result = ipRanges.BinarySearch2(1500);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Country1", result.Country);
        Assert.Equal(1000, result.Lower);
        Assert.Equal(2000, result.Upper);
    }

    [Fact]
    public void JoinComma_WithValidList_ShouldReturnCommaSeparatedString()
    {
        // Arrange
        var values = new List<string> { "value1", "value2", "value3" };

        // Act
        var result = values.JoinComma();

        // Assert
        Assert.Equal("value1,value2,value3", result);
    }

    [Fact]
    public void JoinComma_WithEmptyList_ShouldReturnEmptyString()
    {
        // Arrange
        var values = new List<string>();

        // Act
        var result = values.JoinComma();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void JoinComma_WithSingleItem_ShouldReturnItem()
    {
        // Arrange
        var values = new List<string> { "value1" };

        // Act
        var result = values.JoinComma();

        // Assert
        Assert.Equal("value1", result);
    }

    [Fact]
    public void JoinComma_WithNullValues_ShouldHandleNulls()
    {
        // Arrange
        var values = new List<string> { "value1", null, "value3" };

        // Act
        var result = values.JoinComma();

        // Assert
        Assert.Equal("value1,,value3", result);
    }
}