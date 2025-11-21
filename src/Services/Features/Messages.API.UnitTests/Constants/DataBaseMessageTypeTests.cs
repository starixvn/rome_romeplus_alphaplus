using AlphaPlus.Features.Messages.API.Constants;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Constants
{
    public class DataBaseMessageTypeTests
    {
        [Fact]
        public void Personal_HasCorrectValue()
        {
            // Assert
            Assert.Equal(1, DataBaseMessageType.Personal);
        }

        [Fact]
        public void Others_HasCorrectValue()
        {
            // Assert
            Assert.Equal(3, DataBaseMessageType.Others);
        }

        [Fact]
        public void Constants_AreUnique()
        {
            // Arrange
            var values = new[] { DataBaseMessageType.Personal, DataBaseMessageType.Others };

            // Assert
            Assert.Equal(values.Length, values.Distinct().Count());
        }

        [Fact]
        public void Constants_ArePositive()
        {
            // Assert
            Assert.True(DataBaseMessageType.Personal > 0);
            Assert.True(DataBaseMessageType.Others > 0);
        }

        [Fact]
        public void Personal_IsLessThanOthers()
        {
            // Assert
            Assert.True(DataBaseMessageType.Personal < DataBaseMessageType.Others);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void Constants_ContainExpectedValues(int expectedValue)
        {
            // Arrange
            var allValues = new[] { DataBaseMessageType.Personal, DataBaseMessageType.Others };

            // Assert
            Assert.Contains(expectedValue, allValues);
        }

        [Fact]
        public void Constants_AreConsistentAcrossAccess()
        {
            // Act
            var personal1 = DataBaseMessageType.Personal;
            var personal2 = DataBaseMessageType.Personal;
            var others1 = DataBaseMessageType.Others;
            var others2 = DataBaseMessageType.Others;

            // Assert
            Assert.Equal(personal1, personal2);
            Assert.Equal(others1, others2);
        }

        [Fact]
        public void Constants_HaveExpectedValues()
        {
            // Assert - Test specific expected values for regression
            Assert.Equal(1, DataBaseMessageType.Personal);
            Assert.Equal(3, DataBaseMessageType.Others);
        }

        [Fact]
        public void Constants_AreIntegers()
        {
            // Assert
            Assert.IsType<int>(DataBaseMessageType.Personal);
            Assert.IsType<int>(DataBaseMessageType.Others);
        }

        [Fact]
        public void ClassHasExpectedNumberOfConstants()
        {
            // Arrange
            var constantFields = typeof(DataBaseMessageType)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly)
                .Where(f => f.IsLiteral && !f.IsInitOnly);

            // Assert
            Assert.Equal(2, constantFields.Count());
        }

        [Fact]
        public void Constants_ArePublicStatic()
        {
            // Arrange
            var type = typeof(DataBaseMessageType);
            var personalField = type.GetField(nameof(DataBaseMessageType.Personal));
            var othersField = type.GetField(nameof(DataBaseMessageType.Others));

            // Assert
            Assert.NotNull(personalField);
            Assert.NotNull(othersField);
            Assert.True(personalField.IsPublic);
            Assert.True(personalField.IsStatic);
            Assert.True(othersField.IsPublic);
            Assert.True(othersField.IsStatic);
        }
    }
}
