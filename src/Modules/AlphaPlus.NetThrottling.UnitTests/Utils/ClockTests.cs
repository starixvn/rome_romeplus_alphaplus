using AlphaPlus.NetThrottling.Utils;
using AutoFixture;

namespace AlphaPlus.NetThrottling.UnitTests.Utils
{
    public class ClockTests
    {
        private readonly Fixture _fixture;

        public ClockTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void UtcNow_ShouldReturnCurrentUtcTime()
        {
            // Arrange
            var clock = new Clock();
            var beforeCall = DateTime.UtcNow;

            // Act
            var result = clock.UtcNow;
            var afterCall = DateTime.UtcNow;

            // Assert
            Assert.True(result >= beforeCall);
            Assert.True(result <= afterCall);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void UtcNow_ShouldReturnDifferentValuesOnSubsequentCalls()
        {
            // Arrange
            var clock = new Clock();

            // Act
            var firstCall = clock.UtcNow;
            Thread.Sleep(1); // Small delay to ensure different timestamps
            var secondCall = clock.UtcNow;

            // Assert
            Assert.True(secondCall > firstCall);
        }

        [Fact]
        public void UtcNow_ShouldReturnUtcTimeKind()
        {
            // Arrange
            var clock = new Clock();

            // Act
            var result = clock.UtcNow;

            // Assert
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void UtcNow_ShouldBeCloseToSystemUtcNow()
        {
            // Arrange
            var clock = new Clock();

            // Act
            var clockTime = clock.UtcNow;
            var systemTime = DateTime.UtcNow;

            // Assert
            var difference = Math.Abs((clockTime - systemTime).TotalMilliseconds);
            Assert.True(difference < 1000, $"Time difference should be less than 1 second, but was {difference}ms");
        }

        [Fact]
        public void UtcNow_ShouldBeMonotonic()
        {
            // Arrange
            var clock = new Clock();
            var times = new List<DateTime>();

            // Act
            for (int i = 0; i < 10; i++)
            {
                times.Add(clock.UtcNow);
                Thread.Sleep(1);
            }

            // Assert
            for (int i = 1; i < times.Count; i++)
            {
                Assert.True(times[i] > times[i - 1], $"Time {i} should be greater than time {i - 1}");
            }
        }

        [Fact]
        public void UtcNow_ShouldHandleHighFrequencyCalls()
        {
            // Arrange
            var clock = new Clock();
            var times = new List<DateTime>();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                times.Add(clock.UtcNow);
            }

            // Assert
            Assert.Equal(1000, times.Count);
            Assert.All(times, time => Assert.Equal(DateTimeKind.Utc, time.Kind));
        }

        [Fact]
        public async Task UtcNow_ShouldBeThreadSafe()
        {
            // Arrange
            var clock = new Clock();
            var results = new List<DateTime>();
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        lock (results)
                        {
                            results.Add(clock.UtcNow);
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(1000, results.Count);
            Assert.All(results, time => Assert.Equal(DateTimeKind.Utc, time.Kind));
        }

        [Fact]
        public void UtcNow_ShouldReturnValidDateTime()
        {
            // Arrange
            var clock = new Clock();

            // Act
            var result = clock.UtcNow;

            // Assert
            Assert.True(result > DateTime.MinValue);
            Assert.True(result < DateTime.MaxValue);
            Assert.NotEqual(default(DateTime), result);
        }

        [Fact]
        public void UtcNow_ShouldBeConsistentWithDateTimeUtcNow()
        {
            // Arrange
            var clock = new Clock();

            // Act
            var clockTime = clock.UtcNow;
            var systemTime = DateTime.UtcNow;

            // Assert
            var difference = Math.Abs((clockTime - systemTime).TotalMilliseconds);
            Assert.True(difference < 100, $"Time difference should be minimal, but was {difference}ms");
        }
    }
}
