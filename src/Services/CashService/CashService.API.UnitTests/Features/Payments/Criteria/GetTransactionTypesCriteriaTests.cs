using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetTransactionTypesCriteriaTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetTransactionTypesCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_TransTypeControl", settingKey);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldAlwaysReturnTrue()
        {
            // Arrange
            var criteria = new GetTransactionTypesCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
} 