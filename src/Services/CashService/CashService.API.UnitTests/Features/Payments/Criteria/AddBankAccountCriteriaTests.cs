using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class AddBankAccountCriteriaTests
    {
        private AddBankAccountRequest CreateValidRequest()
        {
            return new AddBankAccountRequest
            {
                ActorId = 1,
                SubAccId = 1,
                IP = "127.0.0.1",
                Language = "en",
                Creator = "TestUser",
                BankId = 1,
                Status = 1,
                AccountHolderName = "John Doe",
                AccountNumber = "1234567890",
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
            var criteria = new AddBankAccountCriteria(request);

            // Assert
            Assert.Equal(request.Creator, criteria.CreatorName);
            Assert.Equal(request.BankId, criteria.BankId);
            Assert.Equal(request.Status, criteria.Status);
            Assert.Equal(request.AccountHolderName, criteria.AccountHolderName);
            Assert.Equal(request.AccountNumber, criteria.AccountNumber);
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
            var criteria = new AddBankAccountCriteria(CreateValidRequest());

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_Ins", settingKey);
        }

        [Theory]
        [InlineData(1, "John Doe", "1234567890", "Test Account", false, true)]     // Valid case without PromptPay
        [InlineData(0, "John Doe", "1234567890", "Test Account", false, false)]    // Invalid BankId
        [InlineData(1, "", "1234567890", "Test Account", false, false)]            // Empty AccountHolderName
        [InlineData(1, "John Doe", "", "Test Account", false, false)]              // Empty AccountNumber
        [InlineData(1, "John Doe", "1234567890", "Test Account", true, false)]     // Invalid case with PromptPay but missing mobile
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(
            int bankId,
            string accountHolderName,
            string accountNumber,
            string accountNickname,
            bool promptPay,
            bool expectedResult)
        {
            // Arrange
            var request = CreateValidRequest();
            request.BankId = bankId;
            request.AccountHolderName = accountHolderName;
            request.AccountNumber = accountNumber;
            request.AccountNickname = accountNickname;
            request.PromptPay = promptPay;
            request.CountryCode = "";
            request.Mobile = "";

            var criteria = new AddBankAccountCriteria(request);

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

            var criteria = new AddBankAccountCriteria(request);

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

            var criteria = new AddBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Theory]
        [InlineData(256)]
        [InlineData(300)]
        public void IsValid_WhenAccountHolderNameExceedsMaxLength_ShouldReturnFalse(int length)
        {
            // Arrange
            var request = CreateValidRequest();
            request.AccountHolderName = new string('a', length);

            var criteria = new AddBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(150)]
        public void IsValid_WhenAccountNumberExceedsMaxLength_ShouldReturnFalse(int length)
        {
            // Arrange
            var request = CreateValidRequest();
            request.AccountNumber = new string('1', length);

            var criteria = new AddBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("123456")]        // AccountHolderName with only digits
        [InlineData("123456789")]     // AccountHolderName with only digits
        public void IsValid_WhenAccountHolderNameContainsOnlyDigits_ShouldReturnTrue(string accountHolderName)
        {
            // Arrange
            var request = CreateValidRequest();
            request.AccountHolderName = accountHolderName;

            var criteria = new AddBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("abc123")]        // AccountNumber with letters
        [InlineData("123abc")]        // AccountNumber with letters
        public void IsValid_WhenAccountNumberContainsNonDigits_ShouldReturnFalse(string accountNumber)
        {
            // Arrange
            var request = CreateValidRequest();
            request.AccountNumber = accountNumber;

            var criteria = new AddBankAccountCriteria(request);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);  
        }

        [Fact]
        public void Constructor_WhenPromptPayIsFalse_ShouldSetMobileToEmptyString()
        {
            // Arrange
            var request = CreateValidRequest();
            request.PromptPay = false;

            // Act
            var criteria = new AddBankAccountCriteria(request);

            // Assert
            Assert.Equal(string.Empty, criteria.Mobile);
        }

        [Fact]
        public void Constructor_WhenPromptPayIsTrue_ShouldSetMobileToCountryCodeAndMobile()
        {
            // Arrange
            var request = CreateValidRequest();
            request.PromptPay = true;
            request.CountryCode = "66";
            request.Mobile = "123456789";

            // Act
            var criteria = new AddBankAccountCriteria(request);

            // Assert
            Assert.Equal("66-123456789", criteria.Mobile);
        }
    }
} 