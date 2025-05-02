using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class GetProvidersFeesCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetProvidersFeesCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentProviderFee_Sel", result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_ShouldReturnExpectedResult(int providerId, bool expected)
        {
            // Arrange
            var criteria = new GetProvidersFeesCriteria(providerId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Constructor_ShouldSetProviderId()
        {
            // Arrange & Act
            const int expectedProviderId = 42;
            var criteria = new GetProvidersFeesCriteria(expectedProviderId);

            // Assert
            Assert.Equal(expectedProviderId, criteria.ProviderId);
        }
    }
} 