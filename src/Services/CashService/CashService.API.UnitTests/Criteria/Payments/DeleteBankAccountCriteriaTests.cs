using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class DeleteBankAccountCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldSetBankAccountId()
        {
            // Arrange
            const int expectedBankAccountId = 123;

            // Act
            var criteria = new DeleteBankAccountCriteria(expectedBankAccountId);

            // Assert
            Assert.Equal(expectedBankAccountId, criteria.BankAccountId);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new DeleteBankAccountCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_ManagementPayments_DelBankAccount", result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_WhenValidBankAccountId_ShouldReturnTrue(int bankAccountId, bool expectedResult)
        {
            // Arrange
            var criteria = new DeleteBankAccountCriteria(bankAccountId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
} 