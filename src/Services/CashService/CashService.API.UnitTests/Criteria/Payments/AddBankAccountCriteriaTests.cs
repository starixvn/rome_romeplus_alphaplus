using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class AddBankAccountCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidModelProvided_ShouldSetAllProperties()
        {
            // Arrange
            var model = new BankAccountModel
            {
                CreatorId = 123,
                BankId = 456,
                Status = 1,
                AccountHolderName = "John Doe",
                AccountNumber = "1234567890",
                AccountNickname = "My Account",
                TransactionTypeID = "1,2,3",
                Agent_DepositMatchingMode = 1,
                PromptPay = true,
                Mobile = "0123456789"
            };

            // Act
            var criteria = new AddBankAccountCriteria(model);

            // Assert
            Assert.Equal(model.CreatorId, criteria.Creator);
            Assert.Equal(model.BankId, criteria.BankId);
            Assert.Equal(model.Status, criteria.Status);
            Assert.Equal(model.AccountHolderName, criteria.AccountHolderName);
            Assert.Equal(model.AccountNumber, criteria.AccountNumber);
            Assert.Equal(model.AccountNickname, criteria.AccountNickname);
            Assert.Equal(model.TransactionTypeID, criteria.TransactionTypeIDs);
            Assert.Equal(model.Agent_DepositMatchingMode, criteria.DepositMatchingMode);
            Assert.Equal(model.PromptPay, criteria.PromptPay);
            Assert.Equal(model.Mobile, criteria.Mobile);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var model = new BankAccountModel();
            var criteria = new AddBankAccountCriteria(model);

            // Act
            var key = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_Ins", key);
        }

        [Theory]
        [InlineData(1, "1234567890", 1, "John Doe", "My Account", "1,2,3", true)]
        [InlineData(2, "0987654321", 2, "Jane Smith", "Business Acc", "4,5", true)]
        public void IsValid_WhenAllRequiredFieldsProvided_ShouldReturnTrue(
            int bankId, 
            string accountNumber, 
            int creator, 
            string accountHolderName, 
            string accountNickname,
            string transactionTypes,
            bool expectedResult)
        {
            // Arrange
            var model = new BankAccountModel
            {
                BankId = bankId,
                AccountNumber = accountNumber,
                CreatorId = creator,
                AccountHolderName = accountHolderName,
                AccountNickname = accountNickname,
                TransactionTypeID = transactionTypes
            };
            var criteria = new AddBankAccountCriteria(model);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Theory]
        [InlineData(0, "1234567890", 1, "John Doe", "My Account", "1,2,3", false)] // Invalid BankId
        [InlineData(1, "", 1, "John Doe", "My Account", "1,2,3", false)] // Empty AccountNumber
        [InlineData(1, "1234567890", 0, "John Doe", "My Account", "1,2,3", false)] // Invalid Creator
        [InlineData(1, "1234567890", 1, "", "My Account", "1,2,3", false)] // Empty AccountHolderName
        [InlineData(1, "1234567890", 1, "John Doe", "", "1,2,3", false)] // Empty AccountNickname
        [InlineData(1, "1234567890", 1, "John Doe", "My Account", "", false)] // Empty TransactionTypes
        public void IsValid_WhenRequiredFieldsMissing_ShouldReturnFalse(
            int bankId,
            string accountNumber,
            int creator,
            string accountHolderName,
            string accountNickname,
            string transactionTypes,
            bool expectedResult)
        {
            // Arrange
            var model = new BankAccountModel
            {
                BankId = bankId,
                AccountNumber = accountNumber,
                CreatorId = creator,
                AccountHolderName = accountHolderName,
                AccountNickname = accountNickname,
                TransactionTypeID = transactionTypes
            };
            var criteria = new AddBankAccountCriteria(model);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Fact]
        public void ErrorProperties_WhenInitialized_ShouldHaveDefaultValues()
        {
            // Arrange
            var model = new BankAccountModel();
            var criteria = new AddBankAccountCriteria(model);

            // Assert
            Assert.Equal(0, criteria.ErrCode);
            Assert.Equal(string.Empty, criteria.ErrMsg);
        }
    }
} 