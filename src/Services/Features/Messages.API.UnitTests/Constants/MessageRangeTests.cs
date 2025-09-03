using AlphaPlus.Features.Messages.API.Constants;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Constants
{
    public class MessageRangeTests
    {
        [Fact]
        public void Normal_HasCorrectValue()
        {
            // Assert
            Assert.Equal(21, MessageRange.Normal);
        }

        [Fact]
        public void Highlight_HasCorrectValue()
        {
            // Assert
            Assert.Equal(20, MessageRange.Highlight);
        }

        [Fact]
        public void Constants_AreUnique()
        {
            // Arrange
            var values = new[] { MessageRange.Normal, MessageRange.Highlight };

            // Assert
            Assert.Equal(values.Length, values.Distinct().Count());
        }

        [Fact]
        public void Constants_ArePositive()
        {
            // Assert
            Assert.True(MessageRange.Normal > 0);
            Assert.True(MessageRange.Highlight > 0);
        }

        [Fact]
        public void Normal_IsGreaterThanHighlight()
        {
            // Assert - Normal range should be higher than highlight range
            Assert.True(MessageRange.Normal > MessageRange.Highlight);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(21)]
        public void Constants_ContainExpectedValues(int expectedValue)
        {
            // Arrange
            var allValues = new[] { MessageRange.Normal, MessageRange.Highlight };

            // Assert
            Assert.Contains(expectedValue, allValues);
        }

        [Fact]
        public void Constants_AreConsistentAcrossAccess()
        {
            // Act
            var normal1 = MessageRange.Normal;
            var normal2 = MessageRange.Normal;
            var highlight1 = MessageRange.Highlight;
            var highlight2 = MessageRange.Highlight;

            // Assert
            Assert.Equal(normal1, normal2);
            Assert.Equal(highlight1, highlight2);
        }

        [Fact]
        public void Constants_HaveExpectedValues()
        {
            // Assert - Test specific expected values for regression
            Assert.Equal(21, MessageRange.Normal);
            Assert.Equal(20, MessageRange.Highlight);
        }

        [Fact]
        public void Constants_AreIntegers()
        {
            // Assert
            Assert.IsType<int>(MessageRange.Normal);
            Assert.IsType<int>(MessageRange.Highlight);
        }

        [Fact]
        public void Constants_AreCloseInValue()
        {
            // Assert - The ranges should be consecutive or very close
            var difference = Math.Abs(MessageRange.Normal - MessageRange.Highlight);
            Assert.Equal(1, difference);
        }

        [Fact]
        public void ClassHasExpectedNumberOfConstants()
        {
            // Arrange
            var constantFields = typeof(MessageRange)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly)
                .Where(f => f.IsLiteral && !f.IsInitOnly);

            // Assert
            Assert.Equal(2, constantFields.Count());
        }

        [Fact]
        public void Constants_ArePublicStatic()
        {
            // Arrange
            var type = typeof(MessageRange);
            var normalField = type.GetField(nameof(MessageRange.Normal));
            var highlightField = type.GetField(nameof(MessageRange.Highlight));

            // Assert
            Assert.NotNull(normalField);
            Assert.NotNull(highlightField);
            Assert.True(normalField.IsPublic);
            Assert.True(normalField.IsStatic);
            Assert.True(highlightField.IsPublic);
            Assert.True(highlightField.IsStatic);
        }

        [Fact]
        public void Constants_AreInReasonableRange()
        {
            // Assert - Values should be reasonable message range numbers
            Assert.InRange(MessageRange.Normal, 1, 100);
            Assert.InRange(MessageRange.Highlight, 1, 100);
        }

        [Fact]
        public void Constants_AreInExpectedOrder()
        {
            // Arrange
            var values = new[] { MessageRange.Highlight, MessageRange.Normal };
            var sortedValues = values.OrderBy(x => x).ToArray();

            // Assert - Should be in ascending order
            Assert.Equal(values, sortedValues);
        }
    }
}
