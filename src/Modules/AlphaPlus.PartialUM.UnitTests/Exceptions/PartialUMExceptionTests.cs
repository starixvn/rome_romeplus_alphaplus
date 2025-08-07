using AlphaPlus.PartialUM.Exceptions;

namespace AlphaPlus.PartialUM.UnitTests.Exceptions;

public class PartialUMExceptionTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var startTime = "01/01/2024 10:00 AM";
        var endTime = "01/01/2024 11:00 AM";

        // Act
        var exception = new PartialUMException(startTime, endTime);

        // Assert
        Assert.Equal(startTime, exception.StartTime);
        Assert.Equal(endTime, exception.EndTime);
        Assert.Equal(PartialUMException.ErrorCode, 9999);
    }

    [Fact]
    public void Constructor_ShouldInheritFromException()
    {
        // Arrange
        var startTime = "01/01/2024 10:00 AM";
        var endTime = "01/01/2024 11:00 AM";

        // Act
        var exception = new PartialUMException(startTime, endTime);

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("01/01/2024 10:00 AM", "")]
    [InlineData("", "01/01/2024 11:00 AM")]
    public void Constructor_ShouldHandleEmptyOrNullTimes(string startTime, string endTime)
    {
        // Act
        var exception = new PartialUMException(startTime, endTime);

        // Assert
        Assert.Equal(startTime, exception.StartTime);
        Assert.Equal(endTime, exception.EndTime);
    }
}
