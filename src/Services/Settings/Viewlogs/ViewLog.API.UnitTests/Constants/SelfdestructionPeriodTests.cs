using AlphaPlus.Settings.ViewLogs.API.Constants;

namespace AlphaPlus.Settings.ViewLogs.API.UnitTests.Constants;

public class SelfdestructionPeriodTests
{
    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_FifteenMinutes_Value()
    {
        // Act & Assert
        Assert.Equal("15 minutes", SelfdestructionPeriod.FifteenMinutes);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_ThirtyMinutes_Value()
    {
        // Act & Assert
        Assert.Equal("30 minutes", SelfdestructionPeriod.ThirtyMinutes);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_OneHour_Value()
    {
        // Act & Assert
        Assert.Equal("1 hour", SelfdestructionPeriod.OneHour);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_ThreeHours_Value()
    {
        // Act & Assert
        Assert.Equal("3 hours", SelfdestructionPeriod.ThreeHours);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_SixHours_Value()
    {
        // Act & Assert
        Assert.Equal("6 hours", SelfdestructionPeriod.SixHours);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_TwelveHours_Value()
    {
        // Act & Assert
        Assert.Equal("12 hours", SelfdestructionPeriod.TwelveHours);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_OneDay_Value()
    {
        // Act & Assert
        Assert.Equal("1 day", SelfdestructionPeriod.OneDay);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_ThreeDays_Value()
    {
        // Act & Assert
        Assert.Equal("3 days", SelfdestructionPeriod.ThreeDays);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_SevenDays_Value()
    {
        // Act & Assert
        Assert.Equal("7 days", SelfdestructionPeriod.SevenDays);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_FifteenDays_Value()
    {
        // Act & Assert
        Assert.Equal("15 days", SelfdestructionPeriod.FifteenDays);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_Correct_ThirtyDays_Value()
    {
        // Act & Assert
        Assert.Equal("30 days", SelfdestructionPeriod.ThirtyDays);
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_All_Unique_Period_Values()
    {
        // Arrange
        var periods = new[]
        {
            SelfdestructionPeriod.FifteenMinutes,
            SelfdestructionPeriod.ThirtyMinutes,
            SelfdestructionPeriod.OneHour,
            SelfdestructionPeriod.ThreeHours,
            SelfdestructionPeriod.SixHours,
            SelfdestructionPeriod.TwelveHours,
            SelfdestructionPeriod.OneDay,
            SelfdestructionPeriod.ThreeDays,
            SelfdestructionPeriod.SevenDays,
            SelfdestructionPeriod.FifteenDays,
            SelfdestructionPeriod.ThirtyDays
        };

        // Act & Assert
        Assert.Equal(periods.Length, periods.Distinct().Count());
    }

    [Fact]
    public void SelfdestructionPeriod_Should_Have_All_NonEmpty_Values()
    {
        // Act & Assert
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.FifteenMinutes));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.ThirtyMinutes));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.OneHour));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.ThreeHours));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.SixHours));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.TwelveHours));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.OneDay));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.ThreeDays));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.SevenDays));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.FifteenDays));
        Assert.False(string.IsNullOrEmpty(SelfdestructionPeriod.ThirtyDays));
    }

    [Theory]
    [InlineData("15 minutes")]
    [InlineData("30 minutes")]
    [InlineData("1 hour")]
    [InlineData("3 hours")]
    [InlineData("6 hours")]
    [InlineData("12 hours")]
    [InlineData("1 day")]
    [InlineData("3 days")]
    [InlineData("7 days")]
    [InlineData("15 days")]
    [InlineData("30 days")]
    public void SelfdestructionPeriod_Should_Contain_Time_Units(string period)
    {
        // Act & Assert
        Assert.True(period.Contains("minutes") || period.Contains("hour") || period.Contains("hours") || period.Contains("day") || period.Contains("days"));
    }
}
