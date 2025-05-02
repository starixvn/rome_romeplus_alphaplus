using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Constants;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class SetTransactionLimitCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidParametersProvided_ShouldSetProperties()
        {
            // Arrange
            const int expectedCustId = 123;
            const int expectedDailyWithdrawalCount = 5;
            const decimal expectedMinPerDeposit = 100m;
            const decimal expectedMaxPerDeposit = 1000m;
            const decimal expectedMinPerWithdrawal = 50m;
            const decimal expectedMaxPerWithdrawal = 500m;

            // Act
            var criteria = new SetTransactionLimitCriteria(
                expectedCustId,
                expectedDailyWithdrawalCount,
                expectedMinPerDeposit,
                expectedMaxPerDeposit,
                expectedMinPerWithdrawal,
                expectedMaxPerWithdrawal);

            // Assert
            Assert.Equal(expectedCustId, criteria.CustId);
            Assert.Equal(expectedDailyWithdrawalCount, criteria.DailyWithdrawalCount);
            Assert.Equal(expectedMinPerDeposit, criteria.MinPerDeposit);
            Assert.Equal(expectedMaxPerDeposit, criteria.MaxPerDeposit);
            Assert.Equal(expectedMinPerWithdrawal, criteria.MinPerWithdrawal);
            Assert.Equal(expectedMaxPerWithdrawal, criteria.MaxPerWithdrawal);
            Assert.Equal(ModeViewConstants.BankAccount, criteria.ModeAction);
            Assert.Equal(string.Empty, criteria.ErrMsg);
            Assert.Equal(0, criteria.ErrCode);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var criteria = new SetTransactionLimitCriteria(1, 5, 100m, 1000m, 50m, 500m);
            const string expectedKey = "Age_CashService_DailyWithdrawLimit_Upd";

            // Act
            var actualKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, actualKey);
        }

        [Theory]
        [InlineData(1, 0, 0, 100, 0, 100, true)]  // Valid minimum values
        [InlineData(1, 5, 100, 1000, 50, 500, true)]  // Valid typical values
        [InlineData(0, 5, 100, 1000, 50, 500, false)]  // Invalid CustId
        [InlineData(1, -1, 100, 1000, 50, 500, false)]  // Invalid DailyWithdrawalCount
        [InlineData(1, 5, -100, 1000, 50, 500, false)]  // Invalid MinPerDeposit
        [InlineData(1, 5, 100, -1000, 50, 500, false)]  // Invalid MaxPerDeposit
        [InlineData(1, 5, 100, 1000, -50, 500, false)]  // Invalid MinPerWithdrawal
        [InlineData(1, 5, 100, 1000, 50, -500, false)]  // Invalid MaxPerWithdrawal
        public void IsValid_WhenValidParameters_ShouldReturnTrue(
            int custId, 
            int dailyWithdrawalCount,
            decimal minPerDeposit,
            decimal maxPerDeposit,
            decimal minPerWithdrawal,
            decimal maxPerWithdrawal,
            bool expectedResult)
        {
            // Arrange
            var criteria = new SetTransactionLimitCriteria(
                custId,
                dailyWithdrawalCount,
                minPerDeposit,
                maxPerDeposit,
                minPerWithdrawal,
                maxPerWithdrawal);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 