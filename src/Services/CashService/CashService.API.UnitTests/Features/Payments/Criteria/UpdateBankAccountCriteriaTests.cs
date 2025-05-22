using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class UpdateBankAccountCriteriaTests
    {
        private UpdateBankAccountRequest CreateValidRequest()
        {
            return new UpdateBankAccountRequest
            {
                ActorId = 1,
                SubAccId = 1,
                IP = "127.0.0.1",
                Language = "en",
                Id = 1,
                Status = 1,
                AccountNickname = "Test Account",
                Agent_TransTypeList = "1,2,3",
                PromptPay = false,
                CountryCode = "66",
                Mobile = "123456789",
                Agent_DepositMatchingMode = 1
            };
        }

        [Fact]
        public void Constructor_WhenValidRequestProvided_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var criteria = new UpdateBankAccountCriteria(request);

            // Assert
            Assert.Equal(request.Id, criteria.Id);
            Assert.Equal(request.Status, criteria.Status);
            Assert.Equal(request.AccountNickname, criteria.AccountNickname);
            Assert.Equal(request.Agent_TransTypeList, criteria.TransactionTypeIDs);
            Assert.Equal(request.PromptPay, criteria.PromptPay);
            Assert.Equal(request.CountryCode + "-" + request.Mobile, criteria.Mobile);
            Assert.Equal(request.Agent_DepositMatchingMode, criteria.DepositMatchingMode);
            Assert.Equal(request.CountryCode, criteria.CountryCode);
            Assert.Equal(request.Mobile, criteria.MobileNumber);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new UpdateBankAccountCriteria(CreateValidRequest());

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_Upd", settingKey);
        }

        [Theory]
        [InlineData(1, "Test Account", false, true)]     // Valid case without PromptPay
        [InlineData(0, "Test Account", false, false)]    // Invalid Id
        [InlineData(1, "", false, true)]                 // Empty AccountNickname is valid
        [InlineData(1, "Test Account", true, false)]     // Invalid case with PromptPay but missing mobile
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(
            int id, 
            string accountNickname, 
            bool promptPay, 
            bool expectedResult)
        {
            // Arrange
            var request = CreateValidRequest();
            request.Id = id;
            request.AccountNickname = accountNickname;
            request.PromptPay = promptPay;

            var criteria = new UpdateBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Fact]
        public void IsValid_WhenPromptPayEnabledWithValidMobile_ShouldReturnTrue()
        {
            // Arrange
            var request = CreateValidRequest();
            request.PromptPay = true;
            request.CountryCode = "66";
            request.Mobile = "123456789";

            var criteria = new UpdateBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("", "123456789", false)]      // Empty country code
        [InlineData("66", "", false)]             // Empty mobile
        [InlineData("abc", "123456789", false)]   // Non-digit country code
        [InlineData("66", "abc123", false)]       // Non-digit mobile
        public void IsValid_WhenPromptPayEnabledWithInvalidMobile_ShouldReturnFalse(
            string countryCode, 
            string mobile, 
            bool expectedResult)
        {
            // Arrange
            var request = CreateValidRequest();
            request.PromptPay = true;
            request.CountryCode = countryCode;
            request.Mobile = mobile;

            var criteria = new UpdateBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Theory]
        [InlineData(256)]
        [InlineData(300)]
        public void IsValid_WhenAccountNicknameExceedsMaxLength_ShouldReturnFalse(int length)
        {
            // Arrange
            var request = CreateValidRequest();
            request.AccountNickname = new string('a', length);

            var criteria = new UpdateBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }
    }
} 