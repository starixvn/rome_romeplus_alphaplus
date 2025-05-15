using AlphaPlus.CashService.API.Features.Payments.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Models.Payments
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
                Status = 2,
                MinAmount = 100.50m,
                MaxAmount = 1000.75m,
                IsEnabled = true
            };

            // Act
            var result = model.ToJsonString();

            // Assert
            var expected = "{\"MethodID\": 1, \"Status\": 2, \"MinAmount\": 100.50, \"MaxAmount\": 1000.75, \"Enabled\": 1}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToJsonString_WhenIsEnabledFalse_ReturnsEnabledAsZero()
        {
            // Arrange
            var model = new ProviderSettingsModel
            {
                Method = 1,
                Status = 2,
                MinAmount = 100.50m,
                MaxAmount = 1000.75m,
                IsEnabled = false
            };

            // Act
            var result = model.ToJsonString();

            // Assert
            var expected = "{\"MethodID\": 1, \"Status\": 2, \"MinAmount\": 100.50, \"MaxAmount\": 1000.75, \"Enabled\": 0}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Constructor_WhenInitialized_SetsDefaultValues()
        {
            // Arrange & Act
            var model = new ProviderSettingsModel();

            // Assert
            Assert.Equal("", model.MethodName);
            Assert.Equal(0, model.CustId);
            Assert.Equal(0, model.ProviderId);
            Assert.Equal(0, model.Method);
            Assert.Equal(0, model.Status);
            Assert.Equal(0, model.TransactionType);
            Assert.Equal(0m, model.MinAmount);
            Assert.Equal(0m, model.MaxAmount);
            Assert.Equal(0m, model.MinAmountTrans);
            Assert.Equal(0m, model.MaxAmountTrans);
            Assert.False(model.IsEnabled);
            Assert.False(model.BankRequired);
        }

        [Fact]
        public void Properties_WhenSet_RetainAssignedValues()
        {
            // Arrange
            var model = new ProviderSettingsModel
            {
                CustId = 123,
                ProviderId = 456,
                Method = 789,
                MethodName = "Test Method",
                Status = 1,
                TransactionType = 2,
                MinAmount = 100m,
                MaxAmount = 1000m,
                MinAmountTrans = 50m,
                MaxAmountTrans = 500m,
                IsEnabled = true,
                BankRequired = true
            };

            // Assert
            Assert.Equal(123, model.CustId);
            Assert.Equal(456, model.ProviderId);
            Assert.Equal(789, model.Method);
            Assert.Equal("Test Method", model.MethodName);
            Assert.Equal(1, model.Status);
            Assert.Equal(2, model.TransactionType);
            Assert.Equal(100m, model.MinAmount);
            Assert.Equal(1000m, model.MaxAmount);
            Assert.Equal(50m, model.MinAmountTrans);
            Assert.Equal(500m, model.MaxAmountTrans);
            Assert.True(model.IsEnabled);
            Assert.True(model.BankRequired);
        }
    }
} 