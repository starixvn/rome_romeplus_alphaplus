using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class GetMobilePrefixesCriteriaTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetMobilePrefixesCriteria();
            const string expectedKey = "Age_CashService_MobilePrefixes_Sel";

            // Act
            var actualKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, actualKey);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldReturnTrue()
        {
            // Arrange
            var criteria = new GetMobilePrefixesCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
} 