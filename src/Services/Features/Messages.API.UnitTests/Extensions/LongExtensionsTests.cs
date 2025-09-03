using AlphaPlus.Features.Messages.API.Extensions;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Extensions
{
    public class LongExtensionsTests
    {
        [Fact]
        public void ToLocalTime_WithUnixEpoch_ReturnsCorrectDateTime()
        {
            // Arrange
            long unixEpoch = 0; // January 1, 1970 UTC

            // Act
            var result = unixEpoch.ToLocalTime();

            // Assert
            var expectedUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expectedLocal = expectedUtc.ToLocalTime();
            Assert.Equal(expectedLocal.Date, result.Date);
        }

        [Fact]
        public void ToLocalTime_WithSpecificTimestamp_ReturnsCorrectDateTime()
        {
            // Arrange
            // January 1, 2024 00:00:00 UTC = 1704067200000 milliseconds
            long timestamp = 1704067200000;

            // Act
            var result = timestamp.ToLocalTime();

            // Assert
            var expectedUtc = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expectedLocal = expectedUtc.ToLocalTime();
            Assert.Equal(expectedLocal.Year, result.Year);
            Assert.Equal(expectedLocal.Month, result.Month);
            Assert.Equal(expectedLocal.Day, result.Day);
            Assert.Equal(expectedLocal.Hour, result.Hour);
            Assert.Equal(expectedLocal.Minute, result.Minute);
            Assert.Equal(expectedLocal.Second, result.Second);
        }

        [Fact]
        public void ToLocalTime_WithCurrentTimestamp_ReturnsReasonableDateTime()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var unixTimestamp = ((DateTimeOffset)now).ToUnixTimeMilliseconds();

            // Act
            var result = unixTimestamp.ToLocalTime();

            // Assert
            var expectedLocal = now.ToLocalTime();
            Assert.Equal(expectedLocal.Year, result.Year);
            Assert.Equal(expectedLocal.Month, result.Month);
            Assert.Equal(expectedLocal.Day, result.Day);
            Assert.Equal(expectedLocal.Hour, result.Hour);
            Assert.Equal(expectedLocal.Minute, result.Minute);
        }

        [Theory]
        [InlineData(0)] // Unix epoch
        [InlineData(946684800000)] // January 1, 2000 UTC
        [InlineData(1577836800000)] // January 1, 2020 UTC
        [InlineData(1640995200000)] // January 1, 2022 UTC
        public void ToLocalTime_WithVariousTimestamps_ReturnsValidDateTime(long timestamp)
        {
            // Act
            var result = timestamp.ToLocalTime();

            // Assert
            Assert.True(result > DateTime.MinValue);
            Assert.True(result < DateTime.MaxValue);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void ToLocalTime_WithMaxValue_DoesNotThrow()
        {
            // Arrange
            long maxValidTimestamp = 253402300799999; // Near DateTime.MaxValue in milliseconds

            // Act & Assert - Should not throw
            var result = maxValidTimestamp.ToLocalTime();
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void ToLocalTime_WithNegativeValue_ReturnsDateBeforeEpoch()
        {
            // Arrange
            long negativeTimestamp = -86400000; // One day before Unix epoch

            // Act
            var result = negativeTimestamp.ToLocalTime();

            // Assert
            var expectedUtc = new DateTime(1969, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            var expectedLocal = expectedUtc.ToLocalTime();
            Assert.Equal(expectedLocal.Date, result.Date);
        }

        [Fact]
        public void ToLocalTime_WithSmallPositiveValue_ReturnsCorrectTime()
        {
            // Arrange
            long smallTimestamp = 1000; // 1 second after Unix epoch

            // Act
            var result = smallTimestamp.ToLocalTime();

            // Assert
            var expectedUtc = new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Utc);
            var expectedLocal = expectedUtc.ToLocalTime();
            Assert.Equal(expectedLocal, result);
        }

        [Fact]
        public void ToLocalTime_PreservesMillisecondPrecision()
        {
            // Arrange
            long timestampWithMilliseconds = 1704067200123; // January 1, 2024 00:00:00.123 UTC

            // Act
            var result = timestampWithMilliseconds.ToLocalTime();

            // Assert
            Assert.Equal(123, result.Millisecond);
        }

        [Fact]
        public void ToLocalTime_ConsistentResults()
        {
            // Arrange
            long timestamp = 1704067200000;

            // Act
            var result1 = timestamp.ToLocalTime();
            var result2 = timestamp.ToLocalTime();

            // Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void ToLocalTime_WithRecentTimestamp_IsCloseToNow()
        {
            // Arrange
            var recentTime = DateTime.UtcNow.AddMinutes(-5);
            var timestamp = ((DateTimeOffset)recentTime).ToUnixTimeMilliseconds();

            // Act
            var result = timestamp.ToLocalTime();

            // Assert
            var timeDifference = Math.Abs((DateTime.Now - result).TotalMinutes);
            Assert.True(timeDifference < 10, "Converted time should be close to current time");
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(60000)]
        [InlineData(3600000)]
        [InlineData(86400000)]
        public void ToLocalTime_WithIncrementalValues_ShowsExpectedIncrement(long incrementMs)
        {
            // Arrange
            long baseTimestamp = 1704067200000; // January 1, 2024 UTC
            long incrementedTimestamp = baseTimestamp + incrementMs;

            // Act
            var baseResult = baseTimestamp.ToLocalTime();
            var incrementedResult = incrementedTimestamp.ToLocalTime();

            // Assert
            var actualDifference = (incrementedResult - baseResult).TotalMilliseconds;
            Assert.Equal(incrementMs, actualDifference);
        }

        [Fact]
        public void ToLocalTime_ExtensionMethod_WorksOnLongVariable()
        {
            // Arrange
            long timestamp = 1704067200000;

            // Act
            var result = timestamp.ToLocalTime();

            // Assert
            Assert.IsType<DateTime>(result);
            Assert.NotEqual(DateTime.MinValue, result);
        }
    }
}
