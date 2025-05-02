using AlphaPlus.CashService.API.Criteria.Payments;
using Xunit;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class UpdateProviderStatusCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldInitializeProperties()
        {
            // Arrange
            const int expectedCustId = 123;
            const int expectedProviderId = 456;
            const int expectedStatus = 1;

            // Act
            var criteria = new UpdateProviderStatusCriteria(expectedCustId, expectedProviderId, expectedStatus);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
            Assert.Equal(expectedProviderId, criteria.ProviderId);
            Assert.Equal(expectedStatus, criteria.Status);
            Assert.Equal(0, criteria.ErrCode);
            Assert.Equal(string.Empty, criteria.ErrMsg);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(100, 200, true)]
        [InlineData(0, 1, false)]
        [InlineData(1, 0, false)]
        [InlineData(0, 0, false)]
        [InlineData(-1, -1, false)]
        public void IsValid_WhenValidParameters_ShouldReturnTrue(int custId, int providerId, bool expectedResult)
        {
            // Arrange
            var criteria = new UpdateProviderStatusCriteria(custId, providerId, 1);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new UpdateProviderStatusCriteria(1, 1, 1);
            const string expectedKey = "Age_CashService_ProviderStatus_Upd";

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }
    }
} 