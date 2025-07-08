using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetProvidersCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidCustIdProvided_ShouldInitializePropertyCorrectly()
        {
            // Arrange
            int custId = 1;

            // Act
            var criteria = new GetProvidersCriteria(custId);

            // Assert
            Assert.Equal(custId, criteria.CustId);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetProvidersCriteria(1);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentProvider_Sel", settingKey);
        }

        [Theory]
        [InlineData(1, true)]     // Valid case
        [InlineData(0, false)]    // Invalid case
        [InlineData(-1, false)]   // Invalid case
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetProvidersCriteria(custId);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 