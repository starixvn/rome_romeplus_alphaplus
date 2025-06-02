using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetAddPaymentMethodInfoCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidParametersProvided_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            int custId = 1;
            int providerId = 2;
            int transactionType = 3;

            // Act
            var criteria = new GetAddPaymentMethodInfoCriteria(custId, providerId, transactionType);

            // Assert
            Assert.Equal(custId, criteria.CustId);
            Assert.Equal(providerId, criteria.ProviderId);
            Assert.Equal(transactionType, criteria.TransactionType);
        }

        [Fact]
        public void Constructor_WhenTransactionTypeIsZero_ShouldSetTransactionTypeToNull()
        {
            // Arrange
            int custId = 1;
            int providerId = 2;
            int transactionType = 0;

            // Act
            var criteria = new GetAddPaymentMethodInfoCriteria(custId, providerId, transactionType);

            // Assert
            Assert.Null(criteria.TransactionType);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetAddPaymentMethodInfoCriteria(1, 2, 3);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentMethods", settingKey);
        }

        [Theory]
        [InlineData(1, 1, true)]    // Valid case
        [InlineData(-1, 1, false)]  // Invalid CustId
        [InlineData(1, -1, false)]  // Invalid ProviderId
        [InlineData(-1, -1, false)] // Both invalid
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(int custId, int providerId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetAddPaymentMethodInfoCriteria(custId, providerId, 0);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Fact]
        public void ChannelContact_WhenSet_ShouldUpdateValue()
        {
            // Arrange
            var criteria = new GetAddPaymentMethodInfoCriteria(1, 2, 3);
            var expectedContact = "test@example.com";

            // Act
            criteria.ChannelContact = expectedContact;

            // Assert
            Assert.Equal(expectedContact, criteria.ChannelContact);
        }
    }
} 