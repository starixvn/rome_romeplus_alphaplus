using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class UpdateProviderSettingsCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const int expectedCustId = 123;
            const int expectedProviderId = 456;
            const string expectedSettings = "test-settings";

            // Act
            var criteria = new UpdateProviderSettingsCriteria(expectedCustId, expectedProviderId, expectedSettings);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
            Assert.Equal(expectedProviderId, criteria.ProviderId);
            Assert.Equal(expectedSettings, criteria.PaymentMethods);
            Assert.Equal(0, criteria.ErrCode);
            Assert.Equal(string.Empty, criteria.ErrMsg);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new UpdateProviderSettingsCriteria(1, 1, "test");

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentProvider_Upd", result);
        }

        [Theory]
        [InlineData(1, 1, "test", true)]  // Valid case
        [InlineData(0, 1, "test", false)] // Invalid CustId
        [InlineData(1, 0, "test", false)] // Invalid ProviderId
        [InlineData(0, 0, "test", false)] // Both invalid
        public void IsValid_WhenValidParameters_ShouldReturnTrue(
            int custId, 
            int providerId, 
            string settings,
            bool expectedResult)
        {
            // Arrange
            var criteria = new UpdateProviderSettingsCriteria(custId, providerId, settings);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
} 