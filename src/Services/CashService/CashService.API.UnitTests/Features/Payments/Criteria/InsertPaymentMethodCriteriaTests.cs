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
                Method = 1
            };
            var criteria = new InsertPaymentMethodCriteria(request);

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
                Method = 1
            };
            var criteria = new InsertPaymentMethodCriteria(request);

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
                Method = 1
            };
            var criteria = new InsertPaymentMethodCriteria(request);

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
                Method = 1
            };

            // Act
            var criteria = new InsertPaymentMethodCriteria(request);

            // Assert
            Assert.Equal(request.ProviderId, criteria.ProviderId);
            Assert.Equal(request.TransactionType, criteria.TransactionType);
            Assert.Equal(request.Method, criteria.Method);
        }
    }
} 