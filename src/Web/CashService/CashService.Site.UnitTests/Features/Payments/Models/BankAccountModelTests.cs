using AlphaPlus.CashService.Site.Features.Payments.Models;
using Newtonsoft.Json;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Payments.Models
{
    public class BankAccountModelTests
    {
        [Fact]
        public void Constructor_WhenCalled_InitializesDefaultValues()
        {
            // Arrange & Act
            var model = new BankAccountModel();

            // Assert
            Assert.Equal(string.Empty, model.AccountNickname);
            Assert.Equal(string.Empty, model.AccountNumber);
            Assert.Equal(string.Empty, model.AccountHolderName);
            Assert.True(model.LastUpdated <= DateTime.UtcNow);
            Assert.True(model.LastUpdated >= DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void LastUpdatedStr_WhenCalled_ReturnsFormattedDateTime()
        {
            // Arrange
            var model = new BankAccountModel
            {
                LastUpdated = new DateTime(2024, 3, 15, 14, 30, 0)
            };

            // Act
            var result = model.LastUpdatedStr;

            // Assert
            Assert.Equal("03/15/2024 02:30:00 PM", result);
        }

        [Fact]
        public void BankDepositMatchingMode_WhenBankDepositMatchingModeIsNull_ReturnsNull()
        {
            // Arrange
            var model = new BankAccountModel
            {
                Bank_DepositMatchingMode = null
            };

            // Act
            var result = model.BankDepositMatchingMode;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BankDepositMatchingMode_WhenBankDepositMatchingModeIsEmpty_ReturnsNull()
        {
            // Arrange
            var model = new BankAccountModel
            {
                Bank_DepositMatchingMode = string.Empty
            };

            // Act
            var result = model.BankDepositMatchingMode;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BankDepositMatchingMode_WhenValidJson_ReturnsDeserializedObject()
        {
            // Arrange
            var depositMatchingMode = new DepositMatchingModeModel
            {
                SourceType = "TestSource",
                MappingTypes = new[] { 1, 2, 3 },
                SourceTypeName = "TestSourceName"
            };
            var json = JsonConvert.SerializeObject(depositMatchingMode);
            var model = new BankAccountModel
            {
                Bank_DepositMatchingMode = json
            };

            // Act
            var result = model.BankDepositMatchingMode;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(depositMatchingMode.SourceType, result.SourceType);
            Assert.Equal(depositMatchingMode.MappingTypes, result.MappingTypes);
            Assert.Equal(depositMatchingMode.SourceTypeName, result.SourceTypeName);
        }
    }
} 