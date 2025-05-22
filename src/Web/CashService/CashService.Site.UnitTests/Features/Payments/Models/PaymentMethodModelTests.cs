using AlphaPlus.CashService.Site.Features.Payments.Models;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Payments.Models
{
    public class PaymentMethodModelTests
    {
        [Fact]
        public void CreateOptionAll_WhenCalled_ReturnsModelWithDefaultValues()
        {
            // Arrange & Act
            var result = PaymentMethodModel.CreateOptionAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal("All", result.Name);
            Assert.Equal(0, result.CustId);
            Assert.Equal(0, result.ProviderId);
            Assert.Equal(0, result.TransactionType);
            Assert.Equal(0, result.Method);
            Assert.Equal(0, result.Status);
            Assert.Equal(string.Empty, result.APIMethodKey);
            Assert.Equal(string.Empty, result.MerchantID);
            Assert.Equal(string.Empty, result.AppSecret);
        }

        [Fact]
        public void PaymentMethodModel_WhenInitialized_PropertiesHaveDefaultValues()
        {
            // Arrange & Act
            var model = new PaymentMethodModel();

            // Assert
            Assert.Equal(0, model.Id);
            Assert.Equal(string.Empty, model.Name);
            Assert.Equal(0, model.CustId);
            Assert.Equal(0, model.ProviderId);
            Assert.Equal(0, model.TransactionType);
            Assert.Equal(0, model.Method);
            Assert.Equal(0, model.Status);
            Assert.Equal(string.Empty, model.APIMethodKey);
            Assert.Equal(string.Empty, model.MerchantID);
            Assert.Equal(string.Empty, model.AppSecret);
        }

        [Fact]
        public void PaymentMethodModel_WhenPropertiesSet_ValuesAreCorrectlyAssigned()
        {
            // Arrange
            var model = new PaymentMethodModel
            {
                Id = 1,
                Name = "Test Payment",
                CustId = 100,
                ProviderId = 200,
                TransactionType = 1,
                Method = 2,
                Status = 1,
                APIMethodKey = "test-key",
                MerchantID = "merchant-123",
                AppSecret = "secret-456"
            };

            // Assert
            Assert.Equal(1, model.Id);
            Assert.Equal("Test Payment", model.Name);
            Assert.Equal(100, model.CustId);
            Assert.Equal(200, model.ProviderId);
            Assert.Equal(1, model.TransactionType);
            Assert.Equal(2, model.Method);
            Assert.Equal(1, model.Status);
            Assert.Equal("test-key", model.APIMethodKey);
            Assert.Equal("merchant-123", model.MerchantID);
            Assert.Equal("secret-456", model.AppSecret);
        }
    }
} 