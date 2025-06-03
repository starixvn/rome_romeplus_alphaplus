using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Requests;

public class UpdatePaymentMethodRequestTests
{
    [Fact]
    public void IsValid_WhenValidData_ReturnsTrue()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test"
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenInvalidActorId_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 0,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test"
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenInvalidProviderId_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 0,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test"
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenEmptyStrings_ReturnsTrue()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = string.Empty,
            MerchantID = string.Empty,
            AppSecret = string.Empty
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.True(result);
    }
} 