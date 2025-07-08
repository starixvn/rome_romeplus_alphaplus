using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Constants;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class SetDailyWithdrawalCountCriteriaTests
    {
        private SetDailyWithdrawalCountRequest CreateValidRequest()
        {
            return new SetDailyWithdrawalCountRequest
            {
                ActorId = 1,
                SubAccId = 1,
                IP = "127.0.0.1",
                Language = "en",
                Count = 10
            };
        }

        [Fact]
        public void Constructor_WhenValidRequestProvided_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var criteria = new SetDailyWithdrawalCountCriteria(request);

            // Assert
            Assert.Equal(ModeViewConstants.PaymentGateway, criteria.ModeAction);
            Assert.Equal(request.Count, criteria.DailyWithdrawalCount);
            Assert.Equal(0, criteria.MinPerDeposit);
            Assert.Equal(0, criteria.MaxPerDeposit);
            Assert.Equal(0, criteria.MinPerWithdrawal);
            Assert.Equal(0, criteria.MaxPerWithdrawal);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new SetDailyWithdrawalCountCriteria(CreateValidRequest());

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_AgentBankAccountLimit_Upd", settingKey);
        }

        [Theory]
        [InlineData(10, true)]      // Valid case
        [InlineData(1, true)]       // Valid minimum value
        [InlineData(32767, true)]   // Valid maximum value
        [InlineData(0, false)]      // Invalid DailyWithdrawalCount (must be > 0)
        [InlineData(-1, false)]     // Negative DailyWithdrawalCount
        [InlineData(32768, false)]  // DailyWithdrawalCount exceeds max (32767)
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(
            int count,
            bool expectedResult)
        {
            // Arrange
            var request = CreateValidRequest();
            request.Count = count;

            var criteria = new SetDailyWithdrawalCountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Theory]
        [InlineData(0, 1, "127.0.0.1", "en", false)]      // Invalid ActorId (must be > 0)
        [InlineData(1, -1, "127.0.0.1", "en", false)]     // Invalid SubAccId (must be >= 0)
        [InlineData(1, 1, "", "en", false)]               // Invalid IP (must not be empty)
        [InlineData(1, 1, null, "en", false)]             // Invalid IP (must not be null)
        [InlineData(1, 1, "127.0.0.1", "", false)]        // Invalid Language (must not be empty)
        [InlineData(1, 1, "127.0.0.1", null, false)]      // Invalid Language (must not be null)
        [InlineData(1, 1, "127.0.0.1", "en", true)]       // Valid case
        public void IsValid_WhenBasePropertiesInvalid_ShouldReturnFalse(
            int actorId, 
            int subAccId, 
            string ip, 
            string language, 
            bool expectedResult)
        {
            // Arrange
            var request = new SetDailyWithdrawalCountRequest
            {
                ActorId = actorId,
                SubAccId = subAccId,
                IP = ip,
                Language = language,
                Count = 10 // Valid count
            };

            var criteria = new SetDailyWithdrawalCountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
} 