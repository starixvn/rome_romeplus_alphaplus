using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetBanksByTransactionTypeCriteriaTests
    {
        [Theory]
        [InlineData("site1", 1, "site1", 1)]
        [InlineData("site2", 0, "site2", null)]
        [InlineData("site3", 2, "site3", 2)]
        public void Constructor_WhenInitialized_ShouldSetPropertiesCorrectly(
            string site,
            int transactionType,
            string expectedSite,
            int? expectedTransactionType)
        {
            // Arrange & Act
            var criteria = new GetBanksByTransactionTypeCriteria(site, transactionType);

            // Assert
            Assert.Equal(expectedSite, criteria.Site);
            Assert.Equal(expectedTransactionType, criteria.TransactionType);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetBanksByTransactionTypeCriteria("site1", 1);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankList", settingKey);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldReturnTrue()
        {
            // Arrange
            var criteria = new GetBanksByTransactionTypeCriteria("site1", 1);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
} 