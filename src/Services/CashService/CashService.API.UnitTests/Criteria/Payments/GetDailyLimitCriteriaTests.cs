using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class GetDailyLimitCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidParametersProvided_ShouldSetProperties()
        {
            // Arrange
            const int expectedCustId = 123;
            const int expectedModeView = 1;

            // Act
            var criteria = new GetDailyLimitCriteria(expectedCustId, expectedModeView);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
            Assert.Equal(expectedModeView, criteria.ModeView);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new GetDailyLimitCriteria(1, 1);
            const string expectedKey = "Age_CashService_DailyWithdrawLimit_Sel";

            // Act
            var actualKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, actualKey);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_WhenValidCustId_ShouldReturnTrue(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetDailyLimitCriteria(custId, 1);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 