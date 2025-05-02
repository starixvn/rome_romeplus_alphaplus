using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class GetBankAccountByIdCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const int expectedBankAccountId = 123;

            // Act
            var criteria = new GetBankAccountByIdCriteria(expectedBankAccountId);

            // Assert
            Assert.Equal(expectedBankAccountId, criteria.BankAccountId);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetBankAccountByIdCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_Sel", result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_WhenValidBankAccountId_ShouldReturnTrue(int bankAccountId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetBankAccountByIdCriteria(bankAccountId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
} 