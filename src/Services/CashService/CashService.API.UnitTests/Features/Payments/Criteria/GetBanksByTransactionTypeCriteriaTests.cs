using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetBanksByTransactionTypeCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidTransactionTypeProvided_ShouldInitializePropertyCorrectly()
        {
            // Arrange
            int transactionType = 1;

            // Act
            var criteria = new GetBanksByTransactionTypeCriteria(transactionType);

            // Assert
            Assert.Equal(transactionType, criteria.TransactionType);
        }

        [Fact]
        public void Constructor_WhenTransactionTypeIsZero_ShouldSetTransactionTypeToNull()
        {
            // Arrange
            int transactionType = 0;

            // Act
            var criteria = new GetBanksByTransactionTypeCriteria(transactionType);

            // Assert
            Assert.Null(criteria.TransactionType);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetBanksByTransactionTypeCriteria(1);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankList", settingKey);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldAlwaysReturnTrue()
        {
            // Arrange
            var criteria = new GetBanksByTransactionTypeCriteria(1);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
} 