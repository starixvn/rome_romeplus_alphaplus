using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetBankAccountByIdCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidBankAccountIdProvided_ShouldInitializePropertyCorrectly()
        {
            // Arrange
            int bankAccountId = 1;

            // Act
            var criteria = new GetBankAccountByIdCriteria(bankAccountId);

            // Assert
            Assert.Equal(bankAccountId, criteria.BankAccountId);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetBankAccountByIdCriteria(1);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_Sel", settingKey);
        }

        [Theory]
        [InlineData(1, true)]     // Valid case
        [InlineData(0, false)]    // Invalid case
        [InlineData(-1, false)]   // Invalid case
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(int bankAccountId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetBankAccountByIdCriteria(bankAccountId);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 