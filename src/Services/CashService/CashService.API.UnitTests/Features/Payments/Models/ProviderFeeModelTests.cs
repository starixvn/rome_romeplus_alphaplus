using AlphaPlus.CashService.API.Features.Payments.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Models
{
    public class ProviderFeeModelTests
    {
        [Fact]
        public void RateText_WhenRateIsZero_ReturnsZeroPercent()
        {
            // Arrange
            var model = new ProviderFeeModel { Rate = 0 };

            // Act
            var result = model.RateText;

            // Assert
            Assert.Equal("0%", result);
        }

        [Fact]
        public void RateText_WhenRateHasDecimalPlaces_ReturnsFormattedPercent()
        {
            // Arrange
            var model = new ProviderFeeModel { Rate = 12.34m };

            // Act
            var result = model.RateText;

            // Assert
            Assert.Equal("12.34%", result);
        }

        [Fact]
        public void RateText_WhenRateIsWholeNumber_ReturnsFormattedPercent()
        {
            // Arrange
            var model = new ProviderFeeModel { Rate = 15m };

            // Act
            var result = model.RateText;

            // Assert
            Assert.Equal("15%", result);
        }

        [Fact]
        public void Constructor_WhenInitialized_SetsDefaultValues()
        {
            // Act
            var model = new ProviderFeeModel();

            // Assert
            Assert.Equal(0, model.ProviderId);
            Assert.Equal(string.Empty, model.TransactionType);
            Assert.Equal(0, model.MethodId);
            Assert.Equal(string.Empty, model.Method);
            Assert.Equal(0, model.Rate);
        }

        [Fact]
        public void Properties_WhenSet_RetainAssignedValues()
        {
            // Arrange
            var model = new ProviderFeeModel
            {
                ProviderId = 123,
                TransactionType = "Credit",
                MethodId = 456,
                Method = "Visa",
                Rate = 2.5m
            };

            // Assert
            Assert.Equal(123, model.ProviderId);
            Assert.Equal("Credit", model.TransactionType);
            Assert.Equal(456, model.MethodId);
            Assert.Equal("Visa", model.Method);
            Assert.Equal(2.5m, model.Rate);
        }
    }
} 