using Xunit;
using AlphaPlus.CashService.API.Features.Payments.Criteria;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class GetBankAccountsCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const int expectedCustId = 123;
            const int expectedBankId = 456;
            const int expectedTransactionType = 789;
            const int expectedStatus = 1;
            const string expectedSearchText = "test";

            // Act
            var criteria = new GetBankAccountsCriteria(
                expectedCustId,
                expectedBankId,
                expectedTransactionType,
                expectedStatus,
                expectedSearchText);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
            Assert.Equal(expectedBankId, criteria.BankId);
            Assert.Equal(expectedTransactionType.ToString(), criteria.TransactionType);
            Assert.Equal(expectedStatus, criteria.Status);
            Assert.Equal(expectedSearchText, criteria.SearchText);
        }

        [Fact]
        public void Constructor_WhenNullOptionalParameters_ShouldSetDefaultValues()
        {
            // Arrange
            const int expectedCustId = 123;

            // Act
            var criteria = new GetBankAccountsCriteria(expectedCustId);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
            Assert.Null(criteria.BankId);
            Assert.Null(criteria.TransactionType);
            Assert.Null(criteria.Status);
            Assert.Null(criteria.SearchText);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(null)]
        public void Constructor_WhenTransactionTypeIsZeroOrNull_ShouldSetTransactionTypeToNull(int? transactionType)
        {
            // Arrange & Act
            var criteria = new GetBankAccountsCriteria(1, transactionType: transactionType);

            // Assert
            Assert.Null(criteria.TransactionType);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetBankAccountsCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_List", result);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldReturnTrue()
        {
            // Arrange
            var criteria = new GetBankAccountsCriteria(1);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }
    }
} 