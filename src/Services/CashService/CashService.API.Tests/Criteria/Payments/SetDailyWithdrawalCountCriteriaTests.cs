using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Constants;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class SetDailyWithdrawalCountCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidParametersProvided_ShouldSetProperties()
        {
            // Arrange
            const int expectedCustId = 123;
            const int expectedDailyWithdrawalCount = 5;

            // Act
            var criteria = new SetDailyWithdrawalCountCriteria(expectedCustId, expectedDailyWithdrawalCount);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
            Assert.Equal(expectedDailyWithdrawalCount, criteria.DailyWithdrawalCount);
            Assert.Equal(ModeViewConstants.PaymentGateway, criteria.ModeAction);
            Assert.Equal(0, criteria.MinPerDeposit);
            Assert.Equal(0, criteria.MaxPerDeposit);
            Assert.Equal(0, criteria.MinPerWithdrawal);
            Assert.Equal(0, criteria.MaxPerWithdrawal);
            Assert.Equal(string.Empty, criteria.ErrMsg);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new SetDailyWithdrawalCountCriteria(1, 5);
            const string expectedKey = "Age_CashService_DailyWithdrawLimit_Upd";

            // Act
            var actualKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, actualKey);
        }

        [Theory]
        [InlineData(1, 0, true)]
        [InlineData(1, 5, true)]
        [InlineData(1, -1, false)]
        [InlineData(0, 5, false)]
        [InlineData(-1, 5, false)]
        [InlineData(0, -1, false)]
        public void IsValid_WhenValidParameters_ShouldReturnTrue(int custId, int dailyWithdrawalCount, bool expectedResult)
        {
            // Arrange
            var criteria = new SetDailyWithdrawalCountCriteria(custId, dailyWithdrawalCount);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 