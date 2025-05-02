using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class GetBanksByCustCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const int expectedCustId = 123;

            // Act
            var criteria = new GetBanksByCustCriteria(expectedCustId);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetBanksByCustCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_UserBankAccount", result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_WhenValidCustId_ShouldReturnTrue(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetBanksByCustCriteria(custId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
} 