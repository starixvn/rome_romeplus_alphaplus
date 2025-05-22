using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Requests;

public class InsertPaymentMethodRequestTests
{
    [Fact]
    public void IsValid_WhenActorIdAndProviderIdArePositive_ReturnsTrue()
    {
        // Arrange
        var request = new InsertPaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenActorIdIsZero_ReturnsFalse()
    {
        // Arrange
        var request = new InsertPaymentMethodRequest
        {
            ActorId = 0,
            ProviderId = 1
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenProviderIdIsZero_ReturnsFalse()
    {
        // Arrange
        var request = new InsertPaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 0
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenBothIdsAreZero_ReturnsFalse()
    {
        // Arrange
        var request = new InsertPaymentMethodRequest
        {
            ActorId = 0,
            ProviderId = 0
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }
} 