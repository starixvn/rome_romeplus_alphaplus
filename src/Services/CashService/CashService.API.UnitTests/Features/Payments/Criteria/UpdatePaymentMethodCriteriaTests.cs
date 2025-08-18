using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class UpdatePaymentMethodCriteriaTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnsCorrectKey()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };
            var criteria = new UpdatePaymentMethodCriteria(request, true);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentMethod_Config", result);
        }

        [Fact]
        public void IsValid_WhenProviderIdIsZero_ReturnsFalse()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 0,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };
            var criteria = new UpdatePaymentMethodCriteria(request, true);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenProviderIdIsPositive_ReturnsTrue()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };
            var criteria = new UpdatePaymentMethodCriteria(request, true);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Constructor_WhenInitialized_SetsPropertiesCorrectly()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };


            // Act
            var criteria = new UpdatePaymentMethodCriteria(request, true);

            // Assert
            Assert.Equal(request.ProviderId, criteria.ProviderId);
            Assert.Equal(request.TransactionType, criteria.TransactionType);
            Assert.Equal(request.Method, criteria.Method);
        }

        [Fact]
        public void IsValid_WhenActorIdIsZero_ReturnsFalse()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 0,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };
            var criteria = new UpdatePaymentMethodCriteria(request, true);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void APIConfig_WhenInitialized_ReturnsCorrectJsonString()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };


            // Act
            var criteria = new UpdatePaymentMethodCriteria(request, true);

            // Assert
            var expectedJson = "{\"secretKey\":\"test_key\",\"merchantNo\":\"test_merchant\",\"appSecret\":\"test_secret\"}";
            Assert.Equal(expectedJson, criteria.APIConfig);
        }

        [Fact]
        public void APIConfig_WhenEmptyStrings_ReturnsCorrectJsonString()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "",
                MerchantID = "",
                AppSecret = "",
            };

            // Act
            var criteria = new UpdatePaymentMethodCriteria(request, false);

            // Assert
            var expectedJson = "{\"secretKey\":\"\",\"merchantNo\":\"\",\"appSecret\":\"\"}";
            Assert.Equal(expectedJson, criteria.APIConfig);
        }

        [Fact]
        public void APIResult_WhenSetToTrue_ReturnsTrue()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",      
            };

            // Act
            var criteria = new UpdatePaymentMethodCriteria(request, true);

            // Assert
            Assert.True(criteria.ValidationResult);
        }

        [Fact]
        public void APIResult_WhenSetToFalse_ReturnsFalse()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };

            // Act
            var criteria = new UpdatePaymentMethodCriteria(request, false);

            // Assert
            Assert.False(criteria.ValidationResult);
        }
    }
} 