using Xunit;
using AlphaPlus.CashService.API.Features.Payments.Models;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Models
{
    public class ProviderSettingsModelTests
    {
        [Fact]
        public void ToJsonString_WhenAllPropertiesSet_ReturnsCorrectJsonString()
        {
            // Arrange
            var model = new ProviderSettingsModel
            {
                Method = 1,
                TransactionType = 2,
                MinAmount = 100.50m,
                MaxAmount = 1000.75m,
                IsEnabled = true
            };

            // Act
            var result = model.ToJsonString();

            // Assert
            var expected = "{\"MethodID\": 1, \"TransactionType\": 2, \"MinAmount\": 100.50, \"MaxAmount\": 1000.75, \"Enabled\": 1}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToJsonString_WhenDisabled_ReturnsEnabledAsZero()
        {
            // Arrange
            var model = new ProviderSettingsModel
            {
                Method = 1,
                TransactionType = 2,
                MinAmount = 100.50m,
                MaxAmount = 1000.75m,
                IsEnabled = false
            };

            // Act
            var result = model.ToJsonString();

            // Assert
            var expected = "{\"MethodID\": 1, \"TransactionType\": 2, \"MinAmount\": 100.50, \"MaxAmount\": 1000.75, \"Enabled\": 0}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToJsonString_WhenZeroAmounts_ReturnsCorrectJsonString()
        {
            // Arrange
            var model = new ProviderSettingsModel
            {
                Method = 1,
                TransactionType = 2,
                MinAmount = 0m,
                MaxAmount = 0m,
                IsEnabled = true
            };

            // Act
            var result = model.ToJsonString();

            // Assert
            var expected = "{\"MethodID\": 1, \"TransactionType\": 2, \"MinAmount\": 0, \"MaxAmount\": 0, \"Enabled\": 1}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Constructor_WhenInitialized_SetsDefaultValues()
        {
            // Arrange & Act
            var model = new ProviderSettingsModel();

            // Assert
            Assert.Equal("", model.MethodName);
            Assert.False(model.IsEnabled);
            Assert.False(model.BankRequired);
            Assert.Equal(0, model.CustId);
            Assert.Equal(0, model.ProviderId);
            Assert.Equal(0, model.Method);
            Assert.Equal(0, model.Status);
            Assert.Equal(0, model.TransactionType);
            Assert.Equal(0m, model.MinAmount);
            Assert.Equal(0m, model.MaxAmount);
            Assert.Equal(0m, model.MinAmountTrans);
            Assert.Equal(0m, model.MaxAmountTrans);
        }
    }
} 