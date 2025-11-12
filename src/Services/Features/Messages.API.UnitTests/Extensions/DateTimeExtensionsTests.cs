using AlphaPlus.Features.Messages.API.Extensions;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void ToShortDateWithSlash_ReturnsCorrectFormat()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 15);

            // Act
            var result = dateTime.ToShortDateWithSlash();

            // Assert
            Assert.Equal("01/15/2024", result);
        }

        [Theory]
        [InlineData(2024, 1, 1, "01/01/2024")]
        [InlineData(2024, 12, 31, "12/31/2024")]
        [InlineData(2023, 6, 15, "06/15/2023")]
        [InlineData(2000, 2, 29, "02/29/2000")] // Leap year
        public void ToShortDateWithSlash_WithDifferentDates_ReturnsCorrectFormat(int year, int month, int day, string expected)
        {
            // Arrange
            var dateTime = new DateTime(year, month, day);

            // Act
            var result = dateTime.ToShortDateWithSlash();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToLongTime_WithAMTime_ReturnsCorrectFormat()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 15, 9, 30, 45);

            // Act
            var result = dateTime.ToLongTime();

            // Assert
            Assert.Equal("09:30:45 AM", result);
        }

        [Fact]
        public void ToLongTime_WithPMTime_ReturnsCorrectFormat()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 15, 14, 30, 45);

            // Act
            var result = dateTime.ToLongTime();

            // Assert
            Assert.Equal("02:30:45 PM", result);
        }

        [Theory]
        [InlineData(0, 0, 0, "12:00:00 AM")]
        [InlineData(12, 0, 0, "12:00:00 PM")]
        [InlineData(1, 30, 45, "01:30:45 AM")]
        [InlineData(13, 30, 45, "01:30:45 PM")]
        [InlineData(23, 59, 59, "11:59:59 PM")]
        public void ToLongTime_WithDifferentTimes_ReturnsCorrectFormat(int hour, int minute, int second, string expected)
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 15, hour, minute, second);

            // Act
            var result = dateTime.ToLongTime();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsTodate_WithCurrentDate_ReturnsTrue()
        {
            // Arrange
            var currentDate = DateTime.Now;

            // Act
            var result = currentDate.IsTodate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsTodate_WithYesterday_ReturnsFalse()
        {
            // Arrange
            var yesterday = DateTime.Now.AddDays(-1);

            // Act
            var result = yesterday.IsTodate();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsTodate_WithTomorrow_ReturnsFalse()
        {
            // Arrange
            var tomorrow = DateTime.Now.AddDays(1);

            // Act
            var result = tomorrow.IsTodate();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsTodate_WithSameDateDifferentTime_ReturnsTrue()
        {
            // Arrange
            var now = DateTime.Now;
            var sameDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            // Act
            var result = sameDate.IsTodate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsTodate_WithSameDateMidnight_ReturnsTrue()
        {
            // Arrange
            var now = DateTime.Now;
            var midnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            // Act
            var result = midnight.IsTodate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsTodate_WithDifferentMonth_ReturnsFalse()
        {
            // Arrange
            var now = DateTime.Now;
            var differentMonth = now.AddMonths(1);

            // Act
            var result = differentMonth.IsTodate();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsTodate_WithDifferentYear_ReturnsFalse()
        {
            // Arrange
            var now = DateTime.Now;
            var differentYear = now.AddYears(1);

            // Act
            var result = differentYear.IsTodate();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToShortDateWithSlash_WithMinValue_DoesNotThrow()
        {
            // Arrange
            var dateTime = DateTime.MinValue;

            // Act & Assert
            var result = dateTime.ToShortDateWithSlash();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ToLongTime_WithMaxValue_DoesNotThrow()
        {
            // Arrange
            var dateTime = DateTime.MaxValue;

            // Act & Assert
            var result = dateTime.ToLongTime();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ExtensionMethods_CanBeChained()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 15, 14, 30, 45);

            // Act
            var dateResult = dateTime.ToShortDateWithSlash();
            var timeResult = dateTime.ToLongTime();
            var isTodayResult = dateTime.IsTodate();

            // Assert
            Assert.NotNull(dateResult);
            Assert.NotNull(timeResult);
            Assert.IsType<bool>(isTodayResult);
        }
    }
}
