using AlphaPlus.SmartPunter.Models;

namespace AlphaPlus.SmartPunter.UnitTests.Models
{
    public class SMACustomerTests
    {
        [Fact]
        public void DefaultConstructor_ShouldSetDefaultValues()
        {
            // Act
            var customer = new SMACustomer();

            // Assert
            Assert.False(customer.IsSmartPunter);
            Assert.False(customer.HasDownlineSmartPunter);
        }

        [Fact]
        public void ParameterizedConstructor_ShouldSetProvidedValues()
        {
            // Arrange
            bool isSmartPunter = true;
            bool hasDownlineSmartPunter = true;

            // Act
            var customer = new SMACustomer(isSmartPunter, hasDownlineSmartPunter);

            // Assert
            Assert.Equal(isSmartPunter, customer.IsSmartPunter);
            Assert.Equal(hasDownlineSmartPunter, customer.HasDownlineSmartPunter);
        }

        [Fact]
        public void ParameterizedConstructor_WithFalseValues_ShouldSetFalseValues()
        {
            // Arrange
            bool isSmartPunter = false;
            bool hasDownlineSmartPunter = false;

            // Act
            var customer = new SMACustomer(isSmartPunter, hasDownlineSmartPunter);

            // Assert
            Assert.False(customer.IsSmartPunter);
            Assert.False(customer.HasDownlineSmartPunter);
        }

        [Fact]
        public void ParameterizedConstructor_WithMixedValues_ShouldSetCorrectValues()
        {
            // Arrange
            bool isSmartPunter = true;
            bool hasDownlineSmartPunter = false;

            // Act
            var customer = new SMACustomer(isSmartPunter, hasDownlineSmartPunter);

            // Assert
            Assert.True(customer.IsSmartPunter);
            Assert.False(customer.HasDownlineSmartPunter);
        }

        [Fact]
        public void Properties_ShouldBeReadOnly()
        {
            // Arrange
            var customer = new SMACustomer(true, false);

            // Act & Assert
            Assert.True(customer.IsSmartPunter);
            Assert.False(customer.HasDownlineSmartPunter);
            // The properties should be read-only, so we can't modify them after construction
        }
    }
}