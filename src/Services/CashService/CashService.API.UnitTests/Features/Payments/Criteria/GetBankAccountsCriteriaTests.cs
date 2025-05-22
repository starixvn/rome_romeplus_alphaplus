using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetBankAccountsCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidParametersProvided_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            int custId = 1;
            int? bankId = 2;
            int? transactionType = 3;
            int? status = 1;
            string searchText = "test";

            // Act
            var criteria = new GetBankAccountsCriteria(custId, bankId, transactionType, status, searchText);

            // Assert
            Assert.Equal(custId, criteria.CustId);
            Assert.Equal(bankId, criteria.BankId);
            Assert.Equal(transactionType.ToString(), criteria.TransactionType);
            Assert.Equal(status, criteria.Status);
            Assert.Equal(searchText, criteria.SearchText);
        }

        [Fact]
        public void Constructor_WhenTransactionTypeIsZero_ShouldSetTransactionTypeToNull()
        {
            // Arrange
            int custId = 1;
            int? transactionType = 0;

            // Act
            var criteria = new GetBankAccountsCriteria(custId, transactionType: transactionType);

            // Assert
            Assert.Null(criteria.TransactionType);
        }

        [Fact]
        public void Constructor_WhenStatusIsTwo_ShouldSetStatusToNull()
        {
            // Arrange
            int custId = 1;
            int? status = 2;

            // Act
            var criteria = new GetBankAccountsCriteria(custId, status: status);

            // Assert
            Assert.Null(criteria.Status);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetBankAccountsCriteria(1);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_List", settingKey);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldAlwaysReturnTrue()
        {
            // Arrange
            var criteria = new GetBankAccountsCriteria(1);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
} 