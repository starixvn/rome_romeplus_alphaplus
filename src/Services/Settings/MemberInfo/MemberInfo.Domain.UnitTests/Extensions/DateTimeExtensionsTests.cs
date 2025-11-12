using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    private readonly DateTime _testDateTime = new(2024, 3, 15, 14, 30, 45);

    [Fact]
    public void ToShortDateWithHyphen_Should_Format_Date_Correctly()
    {
        // Act
        var result = _testDateTime.ToShortDateWithHyphen();

        // Assert
        Assert.Equal("2024-03-15", result);
    }

    [Fact]
    public void ToShortDateWithSlash_Should_Format_Date_Correctly()
    {
        // Act
        var result = _testDateTime.ToShortDateWithSlash();

        // Assert
        Assert.Equal("03/15/2024", result);
    }

    [Fact]
    public void ToLongDateTime24h_Should_Format_DateTime_Correctly()
    {
        // Act
        var result = _testDateTime.ToLongDateTime24h();

        // Assert
        Assert.Equal("03/15/2024 14:30", result);
    }

    [Fact]
    public void ToLongDateTime12h_Should_Format_DateTime_Correctly()
    {
        // Act
        var result = _testDateTime.ToLongDateTime12h();

        // Assert
        Assert.Equal("03/15/2024 02:30 PM", result);
    }

    [Fact]
    public void ToLongDateTime12h_With_Null_Should_Return_Empty_String()
    {
        // Arrange
        DateTime? nullDateTime = null;

        // Act
        var result = nullDateTime.ToLongDateTime12h();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToLongDateTime12h_With_Value_Should_Format_Correctly()
    {
        // Arrange
        DateTime? dateTime = _testDateTime;

        // Act
        var result = dateTime.ToLongDateTime12h();

        // Assert
        Assert.Equal("03/15/2024 02:30 PM", result);
    }

    [Fact]
    public void ToLongDateTime12hWithHyphen_Should_Format_DateTime_Correctly()
    {
        // Act
        var result = _testDateTime.ToLongDateTime12hWithHyphen();

        // Assert
        Assert.Equal("03/15/2024 02:30:45 PM", result);
    }

    [Fact]
    public void ToMMDDWithSlash_Should_Format_Date_Correctly()
    {
        // Act
        var result = _testDateTime.ToMMDDWithSlash();

        // Assert
        Assert.Equal("03/15", result);
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
    public void FirstDayOfYear_Should_Return_First_Day_Of_Year()
    {
        // Act
        var result = _testDateTime.FirstDayOfYear();

        // Assert
        var expected = new DateTime(2024, 1, 1);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FirstDayOfMonth_Should_Return_First_Day_Of_Month()
    {
        // Act
        var result = _testDateTime.FirstDayOfMonth();

        // Assert
        var expected = new DateTime(2024, 3, 1);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FirstDayOfLastMonth_Should_Return_First_Day_Of_Previous_Month()
    {
        // Act
        var result = _testDateTime.FirstDayOfLastMonth();

        // Assert
        var expected = new DateTime(2024, 2, 1);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FirstDayOfLastMonth_Should_Handle_January_Correctly()
    {
        // Arrange
        var januaryDate = new DateTime(2024, 1, 15);

        // Act
        var result = januaryDate.FirstDayOfLastMonth();

        // Assert
        var expected = new DateTime(2023, 12, 1);
        Assert.Equal(expected, result);
    }
}
