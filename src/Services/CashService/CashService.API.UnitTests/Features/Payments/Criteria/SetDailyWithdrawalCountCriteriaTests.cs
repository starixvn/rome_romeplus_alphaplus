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
        [InlineData(0, false)]      // Invalid DailyWithdrawalCount
        [InlineData(-1, false)]     // Negative DailyWithdrawalCount
        [InlineData(32768, false)]  // DailyWithdrawalCount exceeds max
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
    }
} 