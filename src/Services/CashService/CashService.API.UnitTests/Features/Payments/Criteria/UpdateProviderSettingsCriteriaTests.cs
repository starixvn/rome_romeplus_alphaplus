using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class UpdateProviderSettingsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnsCorrectKey()
        {
            // Arrange
            var request = new UpdateProviderRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1
            };
            var settings = "test_settings";
            var criteria = new UpdateProviderSettingsCriteria(request, settings);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentMethod_Upd", result);
        }

        [Fact]
        public void IsValid_WhenProviderIdIsZero_ReturnsFalse()
        {
            // Arrange
            var request = new UpdateProviderRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 0
            };
            var settings = "test_settings";
            var criteria = new UpdateProviderSettingsCriteria(request, settings);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenProviderIdIsPositive_ReturnsTrue()
        {
            // Arrange
            var request = new UpdateProviderRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1
            };
            var settings = "test_settings";
            var criteria = new UpdateProviderSettingsCriteria(request, settings);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Constructor_WhenInitialized_SetsPropertiesCorrectly()
        {
            // Arrange
            var request = new UpdateProviderRequest
            {
                ActorId = 1,
                SubAccId = 2,
                IP = "127.0.0.1",
                Language = "en",
                ProviderId = 1
            };
            var settings = "test_settings";

            // Act
            var criteria = new UpdateProviderSettingsCriteria(request, settings);

            // Assert
            Assert.Equal(request.ProviderId, criteria.ProviderId);
            Assert.Equal(settings, criteria.PaymentMethods);
        }
    }
} 