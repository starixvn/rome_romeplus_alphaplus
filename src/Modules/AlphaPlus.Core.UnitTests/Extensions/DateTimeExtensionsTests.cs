using AlphaPlus.Core.Extensions;

namespace AlphaPlus.Core.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    private readonly DateTime _testDate = new DateTime(2024, 3, 15, 14, 30, 45);
    private readonly DateTime _testDate2 = new DateTime(2024, 3, 20, 10, 15, 30);

    [Fact]
    public void ToShortDate_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToShortDate();

        // Assert
        Assert.Equal("03152024", result);
    }

    [Fact]
    public void ToShortDateWithSlash_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToShortDateWithSlash();

        // Assert
        Assert.Equal("03/15/2024", result);
    }

    [Fact]
    public void ToLongDateTimeWithSlash_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToLongDateTimeWithSlash();

        // Assert
        Assert.Equal("03/15/2024 02:30:45 PM", result);
    }

    [Fact]
    public void ToLongDateTime24HWithSlash_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToLongDateTime24HWithSlash();

        // Assert
        Assert.Equal("03/15/2024 14:30:45", result);
    }

    [Fact]
    public void ToShortMonthNameAndShortYear_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToShortMonthNameAndShortYear();

        // Assert
        Assert.Equal("Mar-24", result);
    }

    [Fact]
    public void ToShortMonthNameAndYear_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToShortMonthNameAndYear();

        // Assert
        Assert.Equal("Mar-2024", result);
    }

    [Fact]
    public void ToMMDDWithSlash_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToMMDDWithSlash();

        // Assert
        Assert.Equal("03/15", result);
    }

    [Fact]
    public void ToMMYYYYWithSlash_Should_Return_Correct_Format()
    {
        // Act
        var result = _testDate.ToMMYYYYWithSlash();

        // Assert
        Assert.Equal("03/2024", result);
    }

    [Fact]
    public void ToStartOfDay_Should_Return_Start_Of_Day()
    {
        // Act
        var result = _testDate.ToStartOfDay();

        // Assert
        Assert.Equal(new DateTime(2024, 3, 15, 0, 0, 0), result);
    }

    [Fact]
    public void ToEndOfDay_Should_Return_End_Of_Day()
    {
        // Act
        var result = _testDate.ToEndOfDay();

        // Assert
        Assert.Equal(new DateTime(2024, 3, 15, 23, 59, 59), result);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, "2024-03-11")]
    [InlineData(DayOfWeek.Sunday, "2024-03-10")]
    public void StartOfWeek_Should_Return_Correct_Start_Of_Week(DayOfWeek startOfWeek, string expectedDate)
    {
        // Arrange
        var expected = DateTime.Parse(expectedDate);

        // Act
        var result = _testDate.StartOfWeek(startOfWeek);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfWeek_Should_Use_Monday_As_Default()
    {
        // Act
        var result = _testDate.StartOfWeek();

        // Assert
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
    }

    [Fact]
    public void FirstDayOfMonth_Should_Return_First_Day_Of_Month()
    {
        // Act
        var result = _testDate.FirstDayOfMonth();

        // Assert
        Assert.Equal(new DateTime(2024, 3, 1), result);
    }

    [Fact]
    public void LastDayOfMonth_Should_Return_Last_Day_Of_Month()
    {
        // Act
        var result = _testDate.LastDayOfMonth();

        // Assert
        Assert.Equal(new DateTime(2024, 3, 31), result);
    }

    [Fact]
    public void IsSameMonthWith_Should_Return_True_For_Same_Month_And_Year()
    {
        // Arrange
        var sameMonthDate = new DateTime(2024, 3, 10);

        // Act
        var result = _testDate.IsSameMonthWith(sameMonthDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSameMonthWith_Should_Return_False_For_Different_Month()
    {
        // Arrange
        var differentMonthDate = new DateTime(2024, 4, 15);

        // Act
        var result = _testDate.IsSameMonthWith(differentMonthDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSameMonthWith_Should_Return_False_For_Different_Year()
    {
        // Arrange
        var differentYearDate = new DateTime(2023, 3, 15);

        // Act
        var result = _testDate.IsSameMonthWith(differentYearDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEndOfMonth_Should_Return_True_For_Last_Day_Of_Month()
    {
        // Arrange
        var lastDayOfMonth = new DateTime(2024, 3, 31);

        // Act
        var result = lastDayOfMonth.IsEndOfMonth();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEndOfMonth_Should_Return_False_For_Not_Last_Day_Of_Month()
    {
        // Act
        var result = _testDate.IsEndOfMonth();

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void GetMonthDiff_Should_Return_Correct_Difference_Across_Years()
    {
        // Arrange
        var futureDate = new DateTime(2025, 1, 15);

        // Act
        var result = futureDate.GetMonthDiff(_testDate);

        // Assert
        Assert.Equal(10, result); // 1 - 3 + (12 * (2025 - 2024)) = -2 + 12 = 10
    }

    [Fact]
    public void FirstDayOfYear_Should_Return_First_Day_Of_Year()
    {
        // Act
        var result = _testDate.FirstDayOfYear();

        // Assert
        Assert.Equal(new DateTime(2024, 1, 1), result);
    }

    [Fact]
    public void LastDayOfYear_Should_Return_Last_Day_Of_Year()
    {
        // Act
        var result = _testDate.LastDayOfYear();

        // Assert
        Assert.Equal(new DateTime(2024, 12, 31), result);
    }

    [Fact]
    public void GetGreaterDate_Should_Return_LimitedDate_When_FromDate_Is_Less()
    {
        // Act
        var result = _testDate.GetGreaterDate(_testDate2);

        // Assert
        Assert.Equal(_testDate2, result);
    }

    [Fact]
    public void GetGreaterDate_Should_Return_FromDate_When_FromDate_Is_Greater()
    {
        // Act
        var result = _testDate2.GetGreaterDate(_testDate);

        // Assert
        Assert.Equal(_testDate2, result);
    }

    [Fact]
    public void GetGreaterDate_Should_Return_FromDate_When_Dates_Are_Equal()
    {
        // Act
        var result = _testDate.GetGreaterDate(_testDate);

        // Assert
        Assert.Equal(_testDate, result);
    }
}
