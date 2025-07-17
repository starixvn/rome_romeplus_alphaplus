using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class InsertPaymentMethodCriteriaTests
    {
        [Fact]
        public void IsValid_WhenAllPropertiesAreValid_ReturnsTrue()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenProviderIdIsZero_ReturnsFalse()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenBasePropertiesAreInvalid_ReturnsFalse()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ActorId = 0, // Invalid ActorId
                SubAccId = 2,
                IP = "", // Invalid IP
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "test_key",
                MerchantID = "test_merchant",
                AppSecret = "test_secret",
            };
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ReturnsCorrectKey()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentMethod_Config", result);
        }

        [Fact]
        public void Constructor_WhenInitialized_SetsPropertiesCorrectly()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Assert
            Assert.Equal(request.ProviderId, criteria.ProviderId);
            Assert.Equal(request.TransactionType, criteria.TransactionType);
            Assert.Equal(request.Method, criteria.Method);
            Assert.Equal(request.GetAPIConfigJson(), criteria.APIConfig);
        }

        [Fact]
        public void Constructor_WhenInitialized_SetsBasePropertiesCorrectly()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Assert
            Assert.Equal(request.ActorId, criteria.CustId);
            Assert.Equal(request.SubAccId, criteria.SubAccId);
            Assert.Equal(request.IP, criteria.IP);
            Assert.Equal(request.Language, criteria.Language);
        }

        [Fact]
        public void APIConfig_WhenRequestHasAPIConfigProperties_ReturnsCorrectJson()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "api_key_123",
                MerchantID = "merchant_456",
                AppSecret = "secret_789",
            };

            // Act
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Assert
            var expectedJson = "{\"APIMethodKey\":\"api_key_123\",\"MerchantID\":\"merchant_456\",\"AppSecret\":\"secret_789\"}";
            Assert.Equal(expectedJson, criteria.APIConfig);
        }

        [Fact]
        public void APIConfig_WhenRequestHasEmptyAPIConfigProperties_ReturnsCorrectJson()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Assert
            var expectedJson = "{\"APIMethodKey\":\"\",\"MerchantID\":\"\",\"AppSecret\":\"\"}";
            Assert.Equal(expectedJson, criteria.APIConfig);
        }

        [Fact]
        public void APIResult_WhenRequestHasAPIResult_SetsCorrectValue()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, true);

            // Assert
            Assert.True(criteria.ValidationResult);
        }

        [Fact]
        public void APIResult_WhenRequestHasFalseAPIResult_SetsCorrectValue()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
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
            var criteria = new InsertPaymentMethodCriteria(request, false);

            // Assert
            Assert.False(criteria.ValidationResult);
        }
    }
} 