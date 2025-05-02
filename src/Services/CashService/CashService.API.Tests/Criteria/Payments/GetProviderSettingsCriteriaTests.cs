using AlphaPlus.CashService.API.Criteria.Payments;
using Xunit;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class GetProviderSettingsCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldInitializeProperties()
        {
            // Arrange
            const int expectedProviderId = 123;
            const int expectedMethod = 1;
            const int expectedStatus = 2;
            const int expectedTransactionType = 3;

            // Act
            var criteria = new GetProviderSettingsCriteria(expectedProviderId, expectedMethod, expectedStatus, expectedTransactionType);

            // Assert
            Assert.Equal(expectedProviderId, criteria.ProviderId);
            Assert.Equal(expectedMethod, criteria.Method);
            Assert.Equal(expectedStatus, criteria.Status);
            Assert.Equal(expectedTransactionType, criteria.TransactionType);
        }

        [Theory]
        [InlineData(1, 1, 1, 1, true)]
        [InlineData(100, 2, 0, 3, true)]
        [InlineData(0, 1, 1, 1, false)]
        [InlineData(-1, 1, 1, 1, false)]
        public void IsValid_WhenValidParameters_ShouldReturnTrue(int providerId, int method, int status, int transactionType, bool expectedResult)
        {
            // Arrange
            var criteria = new GetProviderSettingsCriteria(providerId, method, status, transactionType);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetProviderSettingsCriteria(1, 1, 1, 1);
            const string expectedKey = "Age_CashService_PaymentProviderDtl_Sel";

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }
    }
} 