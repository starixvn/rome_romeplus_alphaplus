using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class GetBanksByTransactionTypeCriteriaTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(42, 42)]
        [InlineData(0, null)]
        public void Constructor_WhenCalled_ShouldSetTransactionTypeCorrectly(int input, int? expected)
        {
            // Arrange & Act
            var criteria = new GetBanksByTransactionTypeCriteria(input);

            // Assert
            Assert.Equal(expected, criteria.TransactionType);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetBanksByTransactionTypeCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankList", result);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldReturnTrue()
        {
            // Arrange
            var criteria = new GetBanksByTransactionTypeCriteria(1);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }
    }
} 