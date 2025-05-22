using AlphaPlus.CashService.API.Features.Payments.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Models
{
    public class BankAccountModelTests
    {
        [Fact]
        public void Constructor_WhenInitialized_ShouldSetDefaultValues()
        {
            // Arrange & Act
            var bankAccount = new BankAccountModel();

            // Assert
            Assert.Equal(string.Empty, bankAccount.AccountNickname);
            Assert.Equal(string.Empty, bankAccount.AccountNumber);
            Assert.Equal(string.Empty, bankAccount.AccountHolderName);
            Assert.Equal(string.Empty, bankAccount.Mobile);
            Assert.Equal(string.Empty, bankAccount.CountryCode);
            Assert.Equal(string.Empty, bankAccount.Creator);
        }

        [Fact]
        public void TransactionTypes_WhenAccessed_ShouldReturnTransactionTypeID()
        {
            // Arrange
            var bankAccount = new BankAccountModel
            {
                TransactionTypeID = "TEST_TYPE"
            };

            // Act
            var result = bankAccount.TransactionTypes;

            // Assert
            Assert.Equal("TEST_TYPE", result);
        }

        [Fact]
        public void LastUpdatedStr_WhenAccessed_ShouldReturnFormattedDate()
        {
            // Arrange
            var testDate = new DateTime(2024, 3, 15, 14, 30, 0);
            var bankAccount = new BankAccountModel
            {
                LastUpdated = testDate
            };

            // Act
            var result = bankAccount.LastUpdatedStr;

            // Assert
            Assert.Equal("03/15/2024 02:30:00 PM", result);
        }

        [Fact]
        public void Properties_WhenSet_ShouldStoreCorrectValues()
        {
            // Arrange
            var bankAccount = new BankAccountModel
            {
                Id = 1,
                AccountNumber = "1234567890",
                AccountNickname = "Test Account",
                AccountHolderName = "John Doe",
                TransactionTypeID = "TYPE1",
                BankName = "Test Bank",
                BankId = 100,
                Status = 1,
                Code = "CODE1",
                LastUpdated = DateTime.Now,
                PromptPay = true,
                Mobile = "1234567890",
                CountryCode = "TH",
                Creator = "Admin",
                CreatorId = 1,
                Bank_TransTypeList = "TYPE1,TYPE2",
                Agent_TransTypeList = "TYPE3,TYPE4",
                Agent_DepositMatchingMode = 1,
                Bank_DepositMatchingMode = "MODE1",
                SubAccId = 2
            };

            // Assert
            Assert.Equal(1, bankAccount.Id);
            Assert.Equal("1234567890", bankAccount.AccountNumber);
            Assert.Equal("Test Account", bankAccount.AccountNickname);
            Assert.Equal("John Doe", bankAccount.AccountHolderName);
            Assert.Equal("TYPE1", bankAccount.TransactionTypeID);
            Assert.Equal("Test Bank", bankAccount.BankName);
            Assert.Equal(100, bankAccount.BankId);
            Assert.Equal(1, bankAccount.Status);
            Assert.Equal("CODE1", bankAccount.Code);
            Assert.True(bankAccount.PromptPay);
            Assert.Equal("1234567890", bankAccount.Mobile);
            Assert.Equal("TH", bankAccount.CountryCode);
            Assert.Equal("Admin", bankAccount.Creator);
            Assert.Equal(1, bankAccount.CreatorId);
            Assert.Equal("TYPE1,TYPE2", bankAccount.Bank_TransTypeList);
            Assert.Equal("TYPE3,TYPE4", bankAccount.Agent_TransTypeList);
            Assert.Equal(1, bankAccount.Agent_DepositMatchingMode);
            Assert.Equal("MODE1", bankAccount.Bank_DepositMatchingMode);
            Assert.Equal(2, bankAccount.SubAccId);
        }
    }
} 