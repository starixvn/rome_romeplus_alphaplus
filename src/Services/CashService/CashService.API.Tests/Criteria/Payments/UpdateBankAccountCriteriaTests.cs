using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Tests.Criteria.Payments
{
    public class UpdateBankAccountCriteriaTests
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
                AccountNickname = "My Account",
                TransactionTypeID = "1,2,3",
                DepositMatchingMode = 1,
                PromptPay = true,
                Mobile = "0123456789"
            };

            // Act
            var criteria = new UpdateBankAccountCriteria(model);

            // Assert
            Assert.Equal(model.CreatorId, criteria.Creator);
            Assert.Equal(model.BankId, criteria.BankId);
            Assert.Equal(model.Status, criteria.Status);
            Assert.Equal(model.AccountNickname, criteria.AccountNickname);
            Assert.Equal(model.TransactionTypeID, criteria.TransactionTypeIDs);
            Assert.Equal(model.DepositMatchingMode, criteria.DepositMatchingMode);
            Assert.Equal(model.PromptPay, criteria.PromptPay);
            Assert.Equal(model.Mobile, criteria.Mobile);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Arrange
            var model = new BankAccountModel();
            var criteria = new UpdateBankAccountCriteria(model);

            // Act
            var key = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_BankAccountAgent_Upd", key);
        }

        [Theory]
        [InlineData(1, 1, 1, "My Account", "1,2,3", 1, true)] // Valid case
        [InlineData(2, 2, 2, "Business Acc", "4,5,6", 2, true)] // Valid case with different values
        public void IsValid_WhenAllRequiredFieldsProvided_ShouldReturnTrue(
            int bankId,
            int status,
            int creator,
            string accountNickname,
            string transactionTypes,
            int depositMatchingMode,
            bool expectedResult)
        {
            // Arrange
            var model = new BankAccountModel
            {
                BankId = bankId,
                Status = status,
                CreatorId = creator,
                AccountNickname = accountNickname,
                TransactionTypeID = transactionTypes,
                DepositMatchingMode = depositMatchingMode
            };
            var criteria = new UpdateBankAccountCriteria(model);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Theory]
        [InlineData(0, 1, 1, "My Account", "1,2,3", 1, false)] // Invalid BankId
        [InlineData(1, 0, 1, "My Account", "1,2,3", 1, false)] // Invalid Status
        [InlineData(1, 1, 0, "My Account", "1,2,3", 1, false)] // Invalid Creator
        [InlineData(1, 1, 1, "", "1,2,3", 1, false)] // Empty AccountNickname
        [InlineData(1, 1, 1, "My Account", "", 1, false)] // Empty TransactionTypes
        [InlineData(1, 1, 1, "My Account", "1,2,3", 0, false)] // Invalid DepositMatchingMode
        public void IsValid_WhenRequiredFieldsMissing_ShouldReturnFalse(
            int bankId,
            int status,
            int creator,
            string accountNickname,
            string transactionTypes,
            int depositMatchingMode,
            bool expectedResult)
        {
            // Arrange
            var model = new BankAccountModel
            {
                BankId = bankId,
                Status = status,
                CreatorId = creator,
                AccountNickname = accountNickname,
                TransactionTypeID = transactionTypes,
                DepositMatchingMode = depositMatchingMode
            };
            var criteria = new UpdateBankAccountCriteria(model);

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
            var criteria = new UpdateBankAccountCriteria(model);

            // Assert
            Assert.Equal(0, criteria.ErrCode);
            Assert.Equal(string.Empty, criteria.ErrMsg);
        }
    }
} 