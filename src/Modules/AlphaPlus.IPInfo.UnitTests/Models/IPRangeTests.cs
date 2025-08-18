using AlphaPlus.IPInfo.Models;

namespace AlphaPlus.IPInfo.UnitTests.Models;

public class IPRangeTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var lower = 1000L;
        var upper = 2000L;
        var country = "United States";

        // Act
        var ipRange = new IPRange(lower, upper, country);

        // Assert
        Assert.Equal(lower, ipRange.Lower);
        Assert.Equal(upper, ipRange.Upper);
        Assert.Equal(country, ipRange.Country);
    }

    [Theory]
    [InlineData(1000, 2000, 1500, 0)] // Value within range
    [InlineData(1000, 2000, 1000, 0)] // Value at lower bound
    [InlineData(1000, 2000, 2000, 0)] // Value at upper bound
    [InlineData(1000, 2000, 500, 1)]  // Value below range
    [InlineData(1000, 2000, 2500, -1)] // Value above range
    public void CompareTo_WithLongValue_ShouldReturnCorrectComparison(long lower, long upper, long value, int expectedComparison)
    {
        // Arrange
        var ipRange = new IPRange(lower, upper, "Test Country");

        // Act
        var result = ipRange.CompareTo(value);

        // Assert
        Assert.Equal(expectedComparison, result);
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenLowerAndUpperAreLess_ShouldReturnNegativeOne()
    {
        // Arrange
        var range1 = new IPRange(1000, 2000, "Country1");
        var range2 = new IPRange(3000, 4000, "Country2");

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenLowerAndUpperAreGreater_ShouldReturnOne()
    {
        // Arrange
        var range1 = new IPRange(3000, 4000, "Country1");
        var range2 = new IPRange(1000, 2000, "Country2");

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenLowerAndUpperAreEqual_ShouldReturnZero()
    {
        // Arrange
        var range1 = new IPRange(1000, 2000, "Country1");
        var range2 = new IPRange(1000, 2000, "Country2");

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenRangesOverlap_ShouldThrowArgumentException()
    {
        // Arrange
        var range1 = new IPRange(1000, 4000, "Country1");
        var range2 = new IPRange(2000, 3000, "Country2");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => range1.CompareTo(range2));
        Assert.Contains("Incomparable values (overlapping)", exception.Message);
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenRangesOverlapInReverse_ShouldThrowArgumentException()
    {
        // Arrange
        var range1 = new IPRange(2000, 3000, "Country1");
        var range2 = new IPRange(1000, 4000, "Country2");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => range1.CompareTo(range2));
        Assert.Contains("Incomparable values (overlapping)", exception.Message);
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenOneRangeIsContainedWithinAnother_ShouldThrowArgumentException()
    {
        // Arrange
        var range1 = new IPRange(1000, 4000, "Country1");
        var range2 = new IPRange(2000, 3000, "Country2");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => range1.CompareTo(range2));
        Assert.Contains("Incomparable values (overlapping)", exception.Message);
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenRangesAreSeparate_ShouldNotThrowException()
    {
        // Arrange
        var range1 = new IPRange(1000, 2000, "Country1");
        var range2 = new IPRange(3000, 4000, "Country2");

        // Act & Assert
        var result = range1.CompareTo(range2);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_WithNullIPRange_ShouldThrowNullReferenceException()
    {
        // Arrange
        var range1 = new IPRange(1000, 2000, "Country1");

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => range1.CompareTo((IPRange)null));
    }

    [Fact]
    public void CompareTo_WithIPRange_WhenRangesPartiallyOverlap_ShouldThrowArgumentException()
    {
        // Arrange
        var range1 = new IPRange(1000, 3000, "Country1");
        var range2 = new IPRange(2000, 2500, "Country2");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => range1.CompareTo(range2));
        Assert.Contains("Incomparable values (overlapping)", exception.Message);
    }


}