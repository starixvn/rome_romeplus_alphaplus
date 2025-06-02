using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria;

public class DeleteBankAccountCriteriaTests
{
    private DeleteBankAccountRequest CreateValidRequest()
    {
        return new DeleteBankAccountRequest
        {
            ActorId = 1,
            SubAccId = 1,
            IP = "127.0.0.1",
            Language = "en",
            BankAccountId = 1
        };
    }

    [Fact]
    public void GetSettingKey_WhenCalled_ReturnsCorrectKey()
    {
        // Arrange
        var request = CreateValidRequest();
        var criteria = new DeleteBankAccountCriteria(request);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_BankAccountAgent_Del", result);
    }

    [Fact]
    public void IsValid_WhenValidDataProvided_ReturnsTrue()
    {
        // Arrange
        var request = CreateValidRequest();
        var criteria = new DeleteBankAccountCriteria(request);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenInvalidBankAccountId_ReturnsFalse()
    {
        // Arrange
        var request = CreateValidRequest();
        request.BankAccountId = 0;
        var criteria = new DeleteBankAccountCriteria(request);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenNegativeBankAccountId_ReturnsFalse()
    {
        // Arrange
        var request = CreateValidRequest();
        request.BankAccountId = -1;
        var criteria = new DeleteBankAccountCriteria(request);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
} 