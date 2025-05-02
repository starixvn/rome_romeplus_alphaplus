using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class GetPaymentMethodsCriteriaTests
    {
        [Theory]
        [InlineData(1, 1, 1, true)]  // Valid inputs
        [InlineData(0, 0, 1, true)]  // Boundary case - minimum valid values
        [InlineData(-1, 0, 1, false)] // Invalid CustId
        [InlineData(0, -1, 1, false)] // Invalid ProviderId
        public void IsValid_WhenValidParameters_ShouldReturnTrue(int custId, int providerId, int transactionType, bool expectedResult)
        {
            // Arrange
            var criteria = new GetPaymentMethodsCriteria(custId, providerId, transactionType);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Constructor_WhenCalled_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const int custId = 1;
            const int providerId = 2;
            const int transactionType = 3;

            // Act
            var criteria = new GetPaymentMethodsCriteria(custId, providerId, transactionType);

            // Assert
            Assert.Equal(custId, criteria.CustId);
            Assert.Equal(providerId, criteria.ProviderId);
            Assert.Equal(transactionType, criteria.TransactionType);
        }

        [Fact]
        public void Constructor_WhenTransactionTypeIsZero_ShouldSetTransactionTypeToNull()
        {
            // Arrange & Act
            var criteria = new GetPaymentMethodsCriteria(1, 1, 0);

            // Assert
            Assert.Null(criteria.TransactionType);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetPaymentMethodsCriteria(1, 1, 1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentMethods", result);
        }
    }
} 