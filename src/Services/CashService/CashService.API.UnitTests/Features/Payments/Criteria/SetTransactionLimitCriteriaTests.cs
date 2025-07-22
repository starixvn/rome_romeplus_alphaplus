using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Constants;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class SetTransactionLimitCriteriaTests
    {
        private SetTransactionLimitRequest CreateValidRequest()
        {
            return new SetTransactionLimitRequest
            {
                ActorId = 1,
                SubAccId = 1,
                IP = "127.0.0.1",
                Language = "en",
                DailyWithdrawalCount = 10,
                MinPerDepositTransaction = 100,
                MaxPerDepositTransaction = 1000,
                MinPerWithdrawalTransaction = 50,
                MaxPerWithdrawalTransaction = 500
            };
        }

        [Fact]
        public void Constructor_WhenValidRequestProvided_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var criteria = new SetTransactionLimitCriteria(request);

            // Assert
            Assert.Equal(ModeViewConstants.BankAccount, criteria.ModeAction);
            Assert.Equal(request.DailyWithdrawalCount, criteria.DailyWithdrawalCount);
            Assert.Equal(request.MinPerDepositTransaction, criteria.MinPerDeposit);
            Assert.Equal(request.MaxPerDepositTransaction, criteria.MaxPerDeposit);
            Assert.Equal(request.MinPerWithdrawalTransaction, criteria.MinPerWithdrawal);
            Assert.Equal(request.MaxPerWithdrawalTransaction, criteria.MaxPerWithdrawal);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new SetTransactionLimitCriteria(CreateValidRequest());

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_AgentBankAccountLimit_Upd", settingKey);
        }

        [Theory]
        [InlineData(1, true)]      // Minimum valid value
        [InlineData(32767, true)]  // Maximum valid value
        [InlineData(0, false)]     // Invalid: zero
        [InlineData(-1, false)]    // Invalid: negative
        [InlineData(32768, false)] // Invalid: exceeds max
        public void IsValid_WhenDailyWithdrawalCountIsSet_ShouldValidateCorrectly(int dailyWithdrawalCount, bool expectedResult)
        {
            // Arrange
            var request = CreateValidRequest();
            request.DailyWithdrawalCount = dailyWithdrawalCount;

            var criteria = new SetTransactionLimitCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Theory]
        [InlineData(0, 1, "127.0.0.1", "en", false)]     // Invalid CustId
        [InlineData(-1, 1, "127.0.0.1", "en", false)]    // Invalid CustId
        [InlineData(1, -1, "127.0.0.1", "en", false)]    // Invalid SubAccId
        [InlineData(1, 1, "", "en", false)]              // Invalid IP
        [InlineData(1, 1, null, "en", false)]            // Invalid IP
        [InlineData(1, 1, "127.0.0.1", "", false)]       // Invalid Language
        [InlineData(1, 1, "127.0.0.1", null, false)]     // Invalid Language
        [InlineData(1, 0, "127.0.0.1", "en", true)]      // Valid SubAccId (0 is allowed)
        public void IsValid_WhenBaseValidationFails_ShouldReturnFalse(int actorId, int subAccId, string ip, string language, bool expectedResult)
        {
            // Arrange
            var request = CreateValidRequest();
            request.ActorId = actorId;
            request.SubAccId = subAccId;
            request.IP = ip;
            request.Language = language;

            var criteria = new SetTransactionLimitCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Theory]
        [InlineData(10, 100, 1000, 50, 500, true)]      // Valid case
        [InlineData(0, 100, 1000, 50, 500, false)]      // Invalid DailyWithdrawalCount (0)
        [InlineData(-1, 100, 1000, 50, 500, false)]     // Invalid DailyWithdrawalCount (negative)
        [InlineData(32767, 100, 1000, 50, 500, true)]   // Valid DailyWithdrawalCount (max)
        [InlineData(32768, 100, 1000, 50, 500, false)]  // DailyWithdrawalCount exceeds max
        [InlineData(10, -1, 1000, 50, 500, false)]      // Negative MinPerDeposit
        [InlineData(10, 0, 1000, 50, 500, false)]       // Zero MinPerDeposit
        [InlineData(10, 100, -1, 50, 500, false)]       // Negative MaxPerDeposit
        [InlineData(10, 100, 0, 50, 500, false)]        // Zero MaxPerDeposit
        [InlineData(10, 100, 1000, -1, 500, false)]     // Negative MinPerWithdrawal
        [InlineData(10, 100, 1000, 0, 500, false)]      // Zero MinPerWithdrawal
        [InlineData(10, 100, 1000, 50, -1, false)]      // Negative MaxPerWithdrawal
        [InlineData(10, 100, 1000, 50, 0, false)]       // Zero MaxPerWithdrawal
        [InlineData(10, 1000, 100, 50, 500, false)]     // MinPerDeposit > MaxPerDeposit
        [InlineData(10, 100, 1000, 500, 50, false)]     // MinPerWithdrawal > MaxPerWithdrawal
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(
            int dailyWithdrawalCount,
            decimal minPerDeposit,
            decimal maxPerDeposit,
            decimal minPerWithdrawal,
            decimal maxPerWithdrawal,
            bool expectedResult)
        {
            // Arrange
            var request = CreateValidRequest();
            request.DailyWithdrawalCount = dailyWithdrawalCount;
            request.MinPerDepositTransaction = minPerDeposit;
            request.MaxPerDepositTransaction = maxPerDeposit;
            request.MinPerWithdrawalTransaction = minPerWithdrawal;
            request.MaxPerWithdrawalTransaction = maxPerWithdrawal;

            var criteria = new SetTransactionLimitCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 