using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetProvidersFeesCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidProviderIdProvided_ShouldInitializePropertyCorrectly()
        {
            // Arrange
            int providerId = 1;

            // Act
            var criteria = new GetProvidersFeesCriteria(providerId);

            // Assert
            Assert.Equal(providerId, criteria.ProviderId);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetProvidersFeesCriteria(1);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentProviderFee_Sel", settingKey);
        }

        [Theory]
        [InlineData(1, true)]     // Valid case
        [InlineData(0, false)]    // Invalid case
        [InlineData(-1, false)]   // Invalid case
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(int providerId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetProvidersFeesCriteria(providerId);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 