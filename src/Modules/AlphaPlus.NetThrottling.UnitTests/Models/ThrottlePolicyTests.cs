using AlphaPlus.NetThrottling.Models;
using AutoFixture;

namespace AlphaPlus.NetThrottling.UnitTests.Models
{
    public class ThrottlePolicyTests
    {
        private readonly Fixture _fixture;

        public ThrottlePolicyTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var name = "TestPolicy";
            var limit = 100;
            var window = 60;
            var blockDuration = 300;

            // Act
            var policy = new ThrottlePolicy
            {
                Name = name,
                MaxRequests = limit,
                ThrottleDuration = window,
                BlockDuration = blockDuration
            };

            // Assert
            Assert.Equal(name, policy.Name);
            Assert.Equal(limit, policy.MaxRequests);
            Assert.Equal(window, policy.ThrottleDuration);
            Assert.Equal(blockDuration, policy.BlockDuration);
        }

        [Fact]
        public void Constructor_WithDefaultValues_ShouldSetDefaultProperties()
        {
            // Act
            var policy = new ThrottlePolicy();

            // Assert
            Assert.Null(policy.Name);
            Assert.Equal(0, policy.MaxRequests);
            Assert.Equal(0, policy.ThrottleDuration);
            Assert.Equal(0, policy.BlockDuration);
        }

        [Fact]
        public void Properties_WithValidValues_ShouldBeSetCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();

            // Act
            policy.Name = "TestPolicy";
            policy.MaxRequests = 50;
            policy.ThrottleDuration = 30;
            policy.BlockDuration = 180;

            // Assert
            Assert.Equal("TestPolicy", policy.Name);
            Assert.Equal(50, policy.MaxRequests);
            Assert.Equal(30, policy.ThrottleDuration);
            Assert.Equal(180, policy.BlockDuration);
        }

        [Fact]
        public void Properties_WithNullName_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();

            // Act
            policy.Name = null;

            // Assert
            Assert.Null(policy.Name);
        }

        [Fact]
        public void Properties_WithEmptyName_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();

            // Act
            policy.Name = "";

            // Assert
            Assert.Equal("", policy.Name);
        }

        [Fact]
        public void Properties_WithZeroValues_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();

            // Act
            policy.MaxRequests = 0;
            policy.ThrottleDuration = 0;
            policy.BlockDuration = 0;

            // Assert
            Assert.Equal(0, policy.MaxRequests);
            Assert.Equal(0, policy.ThrottleDuration);
            Assert.Equal(0, policy.BlockDuration);
        }

        [Fact]
        public void Properties_WithNegativeValues_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();

            // Act
            policy.MaxRequests = -10;
            policy.ThrottleDuration = -5;
            policy.BlockDuration = -30;

            // Assert
            Assert.Equal(-10, policy.MaxRequests);
            Assert.Equal(-5, policy.ThrottleDuration);
            Assert.Equal(-30, policy.BlockDuration);
        }

        [Fact]
        public void Properties_WithLargeValues_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();

            // Act
            policy.MaxRequests = int.MaxValue;
            policy.ThrottleDuration = int.MaxValue;
            policy.BlockDuration = int.MaxValue;

            // Assert
            Assert.Equal(int.MaxValue, policy.MaxRequests);
            Assert.Equal(int.MaxValue, policy.ThrottleDuration);
            Assert.Equal(int.MaxValue, policy.BlockDuration);
        }

        [Fact]
        public void Properties_WithSpecialCharactersInName_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();
            var nameWithSpecialChars = "Test-Policy!@#$%^&*()_+-=[]{}|;':\",./<>?";

            // Act
            policy.Name = nameWithSpecialChars;

            // Assert
            Assert.Equal(nameWithSpecialChars, policy.Name);
        }

        [Fact]
        public void Properties_WithLongName_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();
            var longName = new string('a', 1000);

            // Act
            policy.Name = longName;

            // Assert
            Assert.Equal(longName, policy.Name);
        }

        [Fact]
        public void Properties_WithUnicodeCharactersInName_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();
            var unicodeName = "TestPolicy测试政策";

            // Act
            policy.Name = unicodeName;

            // Assert
            Assert.Equal(unicodeName, policy.Name);
        }

        [Fact]
        public void Properties_WithWhitespaceInName_ShouldHandleCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();
            var nameWithWhitespace = "  Test Policy  ";

            // Act
            policy.Name = nameWithWhitespace;

            // Assert
            Assert.Equal(nameWithWhitespace, policy.Name);
        }

        [Fact]
        public void Properties_WithTypicalValues_ShouldWorkCorrectly()
        {
            // Arrange
            var policy = new ThrottlePolicy();

            // Act
            policy.Name = "API Rate Limit";
            policy.MaxRequests = 1000;
            policy.ThrottleDuration = 3600; // 1 hour
            policy.BlockDuration = 1800; // 30 minutes

            // Assert
            Assert.Equal("API Rate Limit", policy.Name);
            Assert.Equal(1000, policy.MaxRequests);
            Assert.Equal(3600, policy.ThrottleDuration);
            Assert.Equal(1800, policy.BlockDuration);
        }
    }
}
