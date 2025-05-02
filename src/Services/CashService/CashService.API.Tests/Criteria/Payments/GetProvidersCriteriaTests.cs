using AlphaPlus.CashService.API.Criteria.Payments;
using Xunit;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class GetProvidersCriteriaTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldInitializeProperties()
        {
            // Arrange
            const int expectedCustId = 123;

            // Act
            var criteria = new GetProvidersCriteria(expectedCustId);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_WhenValidCustId_ShouldReturnTrue(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetProvidersCriteria(custId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetProvidersCriteria(1);
            const string expectedKey = "Age_CashService_PaymentProvider_Sel";

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }
    }
} 