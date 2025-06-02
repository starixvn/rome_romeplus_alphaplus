using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetProviderSettingsCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidParametersProvided_ShouldInitializeProperties()
        {
            // Arrange
            int providerId = 1;
            int custId = 2;
            int method = 3;
            int status = 4;
            int transactionType = 5;

            // Act
            var criteria = new GetProviderSettingsCriteria(providerId, custId, method, status, transactionType);

            // Assert
            Assert.Equal(providerId, criteria.ProviderId);
            Assert.Equal(custId, criteria.CustId);
            Assert.Equal(method, criteria.Method);
            Assert.Equal(status, criteria.Status);
            Assert.Equal(transactionType, criteria.TransactionType);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetProviderSettingsCriteria(1, 2, 3, 4, 5);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentProviderDetail_Sel", result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_WhenProviderIdProvided_ShouldReturnExpectedResult(int providerId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetProviderSettingsCriteria(providerId, 2, 3, 4, 5);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
} 