using AlphaPlus.Features.Messages.Utils;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Utils
{
    public class SystemTimeTests
    {
        [Fact]
        public void Now_WhenNotSet_ReturnsCurrentDateTime()
        {
            // Arrange - Reset to ensure clean state
            ResetSystemTime();

            // Act
            var result = SystemTime.Now;
            var actualNow = DateTime.Now;

            // Assert
            var timeDifference = Math.Abs((result - actualNow).TotalSeconds);
            Assert.True(timeDifference < 1, "SystemTime.Now should be very close to DateTime.Now");
        }

        [Fact]
        public void Now_WhenSetToSpecificTime_ReturnsSetTime()
        {
            try
            {
                // Arrange
                var specificTime = new DateTime(2024, 1, 15, 10, 30, 45);

                // Act
                SetSystemTime(specificTime);
                var result = SystemTime.Now;

                // Assert
                Assert.Equal(specificTime, result);
            }
            finally
            {
                // Cleanup
                ResetSystemTime();
            }
        }

        [Fact]
        public void Now_WhenSetMultipleTimes_ReturnsLatestSetTime()
        {
            try
            {
                // Arrange
                var firstTime = new DateTime(2024, 1, 1, 12, 0, 0);
                var secondTime = new DateTime(2024, 2, 1, 14, 30, 0);

                // Act
                SetSystemTime(firstTime);
                var firstResult = SystemTime.Now;

                SetSystemTime(secondTime);
                var secondResult = SystemTime.Now;

                // Assert
                Assert.Equal(firstTime, firstResult);
                Assert.Equal(secondTime, secondResult);
                Assert.NotEqual(firstResult, secondResult);
            }
            finally
            {
                // Cleanup
                ResetSystemTime();
            }
        }

        [Fact]
        public void Now_AfterReset_ReturnsCurrentDateTime()
        {
            try
            {
                // Arrange
                var specificTime = new DateTime(2024, 1, 15, 10, 30, 45);
                SetSystemTime(specificTime);

                // Act
                ResetSystemTime();
                var result = SystemTime.Now;
                var actualNow = DateTime.Now;

                // Assert
                var timeDifference = Math.Abs((result - actualNow).TotalSeconds);
                Assert.True(timeDifference < 1, "After reset, SystemTime.Now should return actual DateTime.Now");
            }
            finally
            {
                // Cleanup
                ResetSystemTime();
            }
        }

        [Theory]
        [InlineData(2020, 1, 1, 0, 0, 0)]
        [InlineData(2024, 12, 31, 23, 59, 59)]
        [InlineData(2023, 6, 15, 12, 30, 45)]
        public void Now_WithVariousSetTimes_ReturnsCorrectTime(int year, int month, int day, int hour, int minute, int second)
        {
            try
            {
                // Arrange
                var testTime = new DateTime(year, month, day, hour, minute, second);

                // Act
                SetSystemTime(testTime);
                var result = SystemTime.Now;

                // Assert
                Assert.Equal(testTime, result);
                Assert.Equal(year, result.Year);
                Assert.Equal(month, result.Month);
                Assert.Equal(day, result.Day);
                Assert.Equal(hour, result.Hour);
                Assert.Equal(minute, result.Minute);
                Assert.Equal(second, result.Second);
            }
            finally
            {
                // Cleanup
                ResetSystemTime();
            }
        }

        [Fact]
        public void Now_WithMinValue_ReturnsMinValue()
        {
            try
            {
                // Arrange & Act
                SetSystemTime(DateTime.MinValue);
                var result = SystemTime.Now;

                // Assert
                Assert.Equal(DateTime.MinValue, result);
            }
            finally
            {
                // Cleanup
                ResetSystemTime();
            }
        }

        [Fact]
        public void Now_WithMaxValue_ReturnsMaxValue()
        {
            try
            {
                // Arrange & Act
                SetSystemTime(DateTime.MaxValue);
                var result = SystemTime.Now;

                // Assert
                Assert.Equal(DateTime.MaxValue, result);
            }
            finally
            {
                // Cleanup
                ResetSystemTime();
            }
        }

        [Fact]
        public void Now_IsThreadSafe_DifferentThreadsCanHaveDifferentTimes()
        {
            // Note: This test verifies the ThreadStatic behavior
            // Each thread should have its own SystemTime value

            var thread1Time = new DateTime(2024, 1, 1);
            var thread2Time = new DateTime(2024, 6, 1);
            DateTime thread1Result = DateTime.MinValue;
            DateTime thread2Result = DateTime.MinValue;

            var thread1 = new Thread(() =>
            {
                SetSystemTime(thread1Time);
                Thread.Sleep(50); // Give time for other thread to set its value
                thread1Result = SystemTime.Now;
            });

            var thread2 = new Thread(() =>
            {
                SetSystemTime(thread2Time);
                Thread.Sleep(50); // Give time for other thread to set its value
                thread2Result = SystemTime.Now;
            });

            // Act
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.Equal(thread1Time, thread1Result);
            Assert.Equal(thread2Time, thread2Result);
            Assert.NotEqual(thread1Result, thread2Result);

            // Cleanup
            ResetSystemTime();
        }

        [Fact]
        public void Now_PropertyAccess_IsConsistent()
        {
            try
            {
                // Arrange
                var testTime = new DateTime(2024, 3, 15, 9, 0, 0);
                SetSystemTime(testTime);

                // Act
                var result1 = SystemTime.Now;
                var result2 = SystemTime.Now;
                var result3 = SystemTime.Now;

                // Assert
                Assert.Equal(result1, result2);
                Assert.Equal(result2, result3);
                Assert.Equal(testTime, result1);
            }
            finally
            {
                // Cleanup
                ResetSystemTime();
            }
        }

        [Fact]
        public void Now_WithCurrentTime_IsVeryCloseToActualTime()
        {
            // Arrange - Don't set any time, use default behavior
            ResetSystemTime();

            // Act
            var systemTimeResult = SystemTime.Now;
            var actualTimeResult = DateTime.Now;

            // Assert
            var difference = Math.Abs((systemTimeResult - actualTimeResult).TotalMilliseconds);
            Assert.True(difference < 100, $"SystemTime.Now should be within 100ms of DateTime.Now, but difference was {difference}ms");
        }

        [Fact]
        public void Now_SetterIsInternal_CannotBeAccessedPublicly()
        {
            // Assert - Verify that the setter is not publicly accessible
            var property = typeof(SystemTime).GetProperty(nameof(SystemTime.Now));
            Assert.NotNull(property);
            Assert.True(property.CanRead);
            
            var setter = property.GetSetMethod();
            Assert.Null(setter); // Public setter should be null since it's internal
        }

        // Helper methods to simulate internal access to SystemTime.Now setter
        private static void SetSystemTime(DateTime dateTime)
        {
            // Using reflection to access the internal setter
            var property = typeof(SystemTime).GetProperty(nameof(SystemTime.Now));
            var setter = property?.GetSetMethod(true); // true to get non-public methods
            setter?.Invoke(null, new object[] { dateTime });
        }

        private static void ResetSystemTime()
        {
            // Reset by setting to null (which will make it use DateTime.Now)
            var field = typeof(SystemTime).GetField("now", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            field?.SetValue(null, null);
        }
    }
}
