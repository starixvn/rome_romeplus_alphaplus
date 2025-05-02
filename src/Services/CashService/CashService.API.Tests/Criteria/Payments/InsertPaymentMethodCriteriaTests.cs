using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class InsertPaymentMethodCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidModelProvided_ShouldSetAllProperties()
        {
            // Arrange
            var model = new PaymentMethodModel
            {
                CustId = 123,
                ProviderId = 456,
                TransactionType = 1,
                Method = 2,
                APIMethodKey = "api-key",
                MerchantID = "merchant-id",
                AppSecret = "app-secret"
            };

            // Act
            var criteria = new InsertPaymentMethodCriteria(model);

            // Assert
            Assert.Equal(model.CustId, criteria.CustId);
            Assert.Equal(model.ProviderId, criteria.ProviderId);
            Assert.Equal(model.TransactionType, criteria.TransactionType);
            Assert.Equal(model.Method, criteria.Method);
            Assert.Equal(model.APIMethodKey, criteria.APIMethodKey);
            Assert.Equal(model.MerchantID, criteria.MerchantID);
            Assert.Equal(model.AppSecret, criteria.AppSecret);
        }

        [Fact]
        public void Constructor_WhenNullStringsProvided_ShouldSetEmptyStrings()
        {
            // Arrange
            var model = new PaymentMethodModel
            {
                CustId = 123,
                ProviderId = 456,
                TransactionType = 1,
                Method = 2,
                APIMethodKey = null,
                MerchantID = null,
                AppSecret = null
            };

            // Act
            var criteria = new InsertPaymentMethodCriteria(model);

            // Assert
            Assert.Equal(string.Empty, criteria.APIMethodKey);
            Assert.Equal(string.Empty, criteria.MerchantID);
            Assert.Equal(string.Empty, criteria.AppSecret);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var model = new PaymentMethodModel();
            var criteria = new InsertPaymentMethodCriteria(model);

            // Act
            var key = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_PaymentProvider_Ins", key);
        }

        [Theory]
        [InlineData(1, 1, true)]  // Valid case
        [InlineData(0, 1, false)] // Invalid CustId
        [InlineData(1, 0, false)] // Invalid ProviderId
        [InlineData(0, 0, false)] // Both invalid
        public void IsValid_WhenValidParameters_ShouldReturnTrue(int custId, int providerId, bool expectedResult)
        {
            // Arrange
            var model = new PaymentMethodModel
            {
                CustId = custId,
                ProviderId = providerId,
                TransactionType = 1,
                Method = 1
            };
            var criteria = new InsertPaymentMethodCriteria(model);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Fact]
        public void ErrorProperties_WhenInitialized_ShouldHaveDefaultValues()
        {
            // Arrange
            var model = new PaymentMethodModel();
            var criteria = new InsertPaymentMethodCriteria(model);

            // Assert
            Assert.Equal(0, criteria.ErrCode);
            Assert.Equal(string.Empty, criteria.ErrMsg);
        }
    }
} 