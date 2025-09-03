using AlphaPlus.Features.Messages.API.Constants;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Constants
{
    public class MessageTypeTests
    {
        [Fact]
        public void Normal_HasCorrectValue()
        {
            // Assert
            Assert.Equal(0, MessageType.Normal);
        }

        [Fact]
        public void Personal_HasCorrectValue()
        {
            // Assert
            Assert.Equal(1, MessageType.Personal);
        }

        [Fact]
        public void Special_HasCorrectValue()
        {
            // Assert
            Assert.Equal(2, MessageType.Special);
        }

        [Fact]
        public void System_HasCorrectValue()
        {
            // Assert
            Assert.Equal(3, MessageType.System);
        }

        [Theory]
        [InlineData(0, "Normal")]
        [InlineData(1, "Personal")]
        [InlineData(2, "Special")]
        [InlineData(3, "System")]
        public void ToString_WithValidTypes_ReturnsCorrectString(int type, string expected)
        {
            // Act
            var result = MessageType.ToString(type);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        [InlineData(10)]
        [InlineData(100)]
        public void ToString_WithInvalidTypes_ReturnsEmptyString(int type)
        {
            // Act
            var result = MessageType.ToString(type);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToString_WithNormalConstant_ReturnsNormal()
        {
            // Act
            var result = MessageType.ToString(MessageType.Normal);

            // Assert
            Assert.Equal("Normal", result);
        }

        [Fact]
        public void ToString_WithPersonalConstant_ReturnsPersonal()
        {
            // Act
            var result = MessageType.ToString(MessageType.Personal);

            // Assert
            Assert.Equal("Personal", result);
        }

        [Fact]
        public void ToString_WithSpecialConstant_ReturnsSpecial()
        {
            // Act
            var result = MessageType.ToString(MessageType.Special);

            // Assert
            Assert.Equal("Special", result);
        }

        [Fact]
        public void ToString_WithSystemConstant_ReturnsSystem()
        {
            // Act
            var result = MessageType.ToString(MessageType.System);

            // Assert
            Assert.Equal("System", result);
        }

        [Fact]
        public void AllConstants_AreUnique()
        {
            // Arrange
            var values = new[] { MessageType.Normal, MessageType.Personal, MessageType.Special, MessageType.System };

            // Act & Assert
            Assert.Equal(values.Length, values.Distinct().Count());
        }

        [Fact]
        public void AllConstants_AreNonNegative()
        {
            // Assert
            Assert.True(MessageType.Normal >= 0);
            Assert.True(MessageType.Personal >= 0);
            Assert.True(MessageType.Special >= 0);
            Assert.True(MessageType.System >= 0);
        }

        [Fact]
        public void Constants_AreInAscendingOrder()
        {
            // Assert
            Assert.True(MessageType.Normal < MessageType.Personal);
            Assert.True(MessageType.Personal < MessageType.Special);
            Assert.True(MessageType.Special < MessageType.System);
        }

        [Fact]
        public void ToString_ReturnsNonNullValues()
        {
            // Act & Assert
            Assert.NotNull(MessageType.ToString(MessageType.Normal));
            Assert.NotNull(MessageType.ToString(MessageType.Personal));
            Assert.NotNull(MessageType.ToString(MessageType.Special));
            Assert.NotNull(MessageType.ToString(MessageType.System));
        }

        [Fact]
        public void ToString_ReturnsNonEmptyValuesForValidTypes()
        {
            // Act & Assert
            Assert.NotEmpty(MessageType.ToString(MessageType.Normal));
            Assert.NotEmpty(MessageType.ToString(MessageType.Personal));
            Assert.NotEmpty(MessageType.ToString(MessageType.Special));
            Assert.NotEmpty(MessageType.ToString(MessageType.System));
        }
    }
}
