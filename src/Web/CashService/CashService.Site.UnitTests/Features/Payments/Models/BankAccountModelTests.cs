using AlphaPlus.CashService.Site.Features.Payments.Models;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Payments.Models;

public class BankAccountModelTests
{
    [Fact]
    public void Constructor_WhenCalled_ShouldInitializeDefaultValues()
    {
        // Act
        var bankAccount = new BankAccountModel();

        // Assert
        bankAccount.AccountNickname.Should().BeEmpty();
        bankAccount.AccountNumber.Should().BeEmpty();
        bankAccount.AccountHolderName.Should().BeEmpty();
        bankAccount.Mobile.Should().BeEmpty();
        bankAccount.MobilePrefix.Should().BeEmpty();
        bankAccount.Creator.Should().BeEmpty();
        bankAccount.LastUpdated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void LastUpdatedStr_WhenDateTimeIsSet_ShouldReturnFormattedDateTime()
    {
        // Arrange
        var bankAccount = new BankAccountModel
        {
            LastUpdated = new DateTime(2024, 3, 15, 14, 30, 0, DateTimeKind.Utc)
        };

        // Act
        var result = bankAccount.LastUpdatedStr;

        // Assert
        result.Should().Be("03/15/2024 02:30:00 PM");
    }

    [Fact]
    public void Properties_WhenSet_ShouldReturnExpectedValues()
    {
        // Arrange
        var bankAccount = new BankAccountModel
        {
            Id = 1,
            AccountNumber = "1234567890",
            AccountNickname = "Main Account",
            AccountHolderName = "John Doe",
            TransactionTypes = "Deposit,Withdrawal",
            BankName = "Test Bank",
            BankId = 1,
            Status = 1,
            DepositMatchingMode = 2,
            PromptPay = true,
            Mobile = "1234567890",
            MobilePrefix = "+1",
            Creator = "Admin",
            CreatorId = 1
        };

        // Assert
        bankAccount.Id.Should().Be(1);
        bankAccount.AccountNumber.Should().Be("1234567890");
        bankAccount.AccountNickname.Should().Be("Main Account");
        bankAccount.AccountHolderName.Should().Be("John Doe");
        bankAccount.TransactionTypes.Should().Be("Deposit,Withdrawal");
        bankAccount.BankName.Should().Be("Test Bank");
        bankAccount.BankId.Should().Be(1);
        bankAccount.Status.Should().Be(1);
        bankAccount.DepositMatchingMode.Should().Be(2);
        bankAccount.PromptPay.Should().BeTrue();
        bankAccount.Mobile.Should().Be("1234567890");
        bankAccount.MobilePrefix.Should().Be("+1");
        bankAccount.Creator.Should().Be("Admin");
        bankAccount.CreatorId.Should().Be(1);
    }
} 