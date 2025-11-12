using AlphaPlus.Settings.Transfer.API.Extensions;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    private readonly DateTime _testDateTime = new(2024, 3, 15, 14, 30, 45);

    [Fact]
    public void ToShortDateWithSlash_Should_Format_Date_Correctly()
    {
        // Act
        var result = _testDateTime.ToShortDateWithSlash();

        // Assert
        Assert.Equal("03/15/2024", result);
    }

    [Fact]
    public void ToLongDateWithSlash_Should_Format_DateTime_Correctly_With_AM_PM()
    {
        // Act
        var result = _testDateTime.ToLongDateWithSlash();

        // Assert
        Assert.Equal("03/15/2024 02:30:45 PM", result);
    }

    [Fact]
    public void ToLongDateWithSlash24HFormat_Should_Format_DateTime_Correctly_With_24H()
    {
        // Act
        var result = _testDateTime.ToLongDateWithSlash24HFormat();

        // Assert
        Assert.Equal("03/15/2024 14:30:45", result);
    }

    [Fact]
    public void GetGreaterDate_Should_Return_LimitedDate_When_FromDate_Is_Earlier()
    {
        // Arrange
        var fromDate = new DateTime(2024, 1, 1);
        var limitedDate = new DateTime(2024, 2, 1);

        // Act
        var result = fromDate.GetGreaterDate(limitedDate);

        // Assert
        Assert.Equal(limitedDate, result);
    }

    [Fact]
    public void GetGreaterDate_Should_Return_FromDate_When_FromDate_Is_Later()
    {
        // Arrange
        var fromDate = new DateTime(2024, 3, 1);
        var limitedDate = new DateTime(2024, 2, 1);

        // Act
        var result = fromDate.GetGreaterDate(limitedDate);

        // Assert
        Assert.Equal(fromDate, result);
    }

    [Fact]
    public void GetGreaterDate_Should_Return_FromDate_When_Dates_Are_Equal()
    {
        // Arrange
        var date = new DateTime(2024, 2, 1);

        // Act
        var result = date.GetGreaterDate(date);

        // Assert
        Assert.Equal(date, result);
    }

    [Fact]
    public void ToShortDateWithSlash_Should_Handle_LeapYear_Date()
    {
        // Arrange
        var leapYearDate = new DateTime(2024, 2, 29);

        // Act
        var result = leapYearDate.ToShortDateWithSlash();

        // Assert
        Assert.Equal("02/29/2024", result);
    }

    [Fact]
    public void ToLongDateWithSlash_Should_Handle_Midnight_Time()
    {
        // Arrange
        var midnightDateTime = new DateTime(2024, 3, 15, 0, 0, 0);

        // Act
        var result = midnightDateTime.ToLongDateWithSlash();

        // Assert
        Assert.Equal("03/15/2024 12:00:00 AM", result);
    }

    [Fact]
    public void ToLongDateWithSlash24HFormat_Should_Handle_Single_Digit_Hours()
    {
        // Arrange
        var singleDigitHourDateTime = new DateTime(2024, 3, 15, 9, 5, 30);

        // Act
        var result = singleDigitHourDateTime.ToLongDateWithSlash24HFormat();

        // Assert
        Assert.Equal("03/15/2024 09:05:30", result);
    }
}
